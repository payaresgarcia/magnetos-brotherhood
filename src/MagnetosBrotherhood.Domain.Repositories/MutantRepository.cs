namespace MagnetosBrotherhood.Domain.Repositories
{
    using AutoMapper;
    using MagnetosBrotherhood.DAL.Interfaces;
    using MagnetosBrotherhood.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MutantRepository : IMutantRepository
    {
        private readonly IMapper _mapper;
        private readonly IMutantDynamoDb _mutantDynamoDbRepository;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantRepository"/> class.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="mutantDynamoDbRepository"></param>
        public MutantRepository(IMapper mapper, IMutantDynamoDb mutantDynamoDbRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mutantDynamoDbRepository = mutantDynamoDbRepository ?? throw new ArgumentNullException(nameof(mutantDynamoDbRepository));
        }

        /// <summary>
        /// Save a sequence of dna and for a human or mutant.
        /// </summary>
        /// <param name="dnaEntry"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task object.</returns>
        public async Task SaveDnaRequestAsync(DnaEntryDo dnaEntry, CancellationToken cancellationToken)
        {
            if (dnaEntry == null)
                throw new ArgumentNullException(nameof(dnaEntry));

            var entry = _mapper.Map<DAL.Models.DnaEntry>(dnaEntry);
            entry.RecordType = Constants.Dna.DnaRecordType;

            await _mutantDynamoDbRepository.CreateDnaEntryAsync(entry, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all dna entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of dnas</returns>
        public async Task<IEnumerable<DnaEntryDo>> GetDnaEntriesAsync(CancellationToken cancellationToken)
        {
            var results = await _mutantDynamoDbRepository.GetDnaEntriesAsync(cancellationToken).ConfigureAwait(false);
            if (results == null)
                return new List<DnaEntryDo>();

            return results.Select(e => _mapper.Map<Domain.Models.DnaEntryDo>(e))?.ToList();
        }
    }
}
