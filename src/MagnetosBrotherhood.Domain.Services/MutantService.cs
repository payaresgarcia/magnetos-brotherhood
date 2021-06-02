namespace MagnetosBrotherhood.Domain.Services
{
    using MagnetosBrotherhood.Domain.Common;
    using MagnetosBrotherhood.Domain.Models;
    using MagnetosBrotherhood.Domain.Repositories;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MutantService : IMutantService
    {
        private readonly IMutantRepository _mutantRepository;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantService"/> class.
        /// </summary>
        /// <param name="mutantRepository"></param>
        public MutantService(IMutantRepository mutantRepository)
        {
            _mutantRepository = mutantRepository ?? throw new ArgumentNullException(nameof(mutantRepository));
        }

        /// <summary>
        /// Process a sequence of dna and determine whether is human or mutant.
        /// </summary>
        /// <param name="dnaInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A true/false flag based on result.</returns>
        public async Task<bool> ProcessDnaSampleAsync(DnaInfo dnaInfo, CancellationToken cancellationToken)
        {
            if (dnaInfo?.Dna is null)
                throw new ArgumentNullException(nameof(dnaInfo));

            if (cancellationToken.IsCancellationRequested)
                return false;

            var result = await IsMutant(dnaInfo.Dna).ConfigureAwait(false);
            await HashAndSaveDnaResult(dnaInfo.Dna, result, cancellationToken);

            return result;
        }

        /// <summary>
        /// Determine whether a dna corresponds to a mutant or not.
        /// </summary>
        /// <param name="dnaArray"></param>
        /// <returns>A bool flag based on whether is mutant or not.</returns>
        public virtual async Task<bool> IsMutant(string[] dnaArray)
        {
            DnaSequence dnaSecequenceInfo = new DnaSequence();

            var dnaMatrix = Common.DnaOperations.ValidateAndStructureDnaInfo(dnaArray);
            if (dnaMatrix is null)
                return false;

            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            
            // Run a task for checking oblique sequences.
            Task ObliqueChecker = Task.Run(() => {
                Common.DnaOperations.CountObliqueMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);
            });

            // Run a task for checking vertical sequences.
            Task VerticalChecker = Task.Run(() => {
                Common.DnaOperations.CountVerticalMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);
            });

            // Run a task for checking horizontal sequences.
            Task HorizontalChecker = Task.Run(() => {
                Common.DnaOperations.CountHorizontalMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);
            });

            // All tasks end when more than 2 sequences are found (across all tasks) or their processes are done.
            await Task.WhenAll(ObliqueChecker, VerticalChecker, HorizontalChecker).ConfigureAwait(false);

            if (dnaSecequenceInfo.SequenceCounter > 1)
                return true;

            return false;
        }

        /// <summary>
        /// Build/Save needed data for storing dna information.
        /// </summary>
        /// <param name="dna"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="isMutant"></param>
        public async Task HashAndSaveDnaResult(string[] dna, bool isMutant, CancellationToken cancellationToken)
        {
            if (dna == null)
                return;

            var bytes = Utilities.GetHashBytes(string.Join("-", dna));
            var base64String = Utilities.ConvertArrayToBase64String(bytes);

            var newDnaEntry = new Domain.Models.DnaEntryDo
            {
                DnaHash = base64String,
                DnaType = isMutant ? Constants.Dna.MutantDna : Constants.Dna.HumanDna,
                CreatedDate = DateTime.UtcNow.ToUniversalTime().ToString("yyyy/MM/dd")
            };

            await _mutantRepository.SaveDnaRequestAsync(newDnaEntry, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get dnas statistics.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A DnaStatisticsDo result.</returns>
        public async Task<DnaStatisticsDo> GetDnaStatisticsAsync(CancellationToken cancellationToken)
        {
            var statistics = new DnaStatisticsDo();

            var results = await _mutantRepository.GetDnaEntriesAsync(cancellationToken).ConfigureAwait(false);
            if (results != null)
            {
                statistics.MutantDnaCount = results.Where(e => e.DnaType == Constants.Dna.MutantDna).Count();
                statistics.HumanDnaCount = results.Where(e => e.DnaType == Constants.Dna.HumanDna).Count();
            }

            return statistics;
        }
    }
}
