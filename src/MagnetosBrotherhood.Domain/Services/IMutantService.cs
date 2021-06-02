namespace MagnetosBrotherhood.Domain.Services
{
    using MagnetosBrotherhood.Domain.Models;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for mutant service.
    /// </summary>
    public interface IMutantService
    {
        /// <summary>
        /// Process a sequence of dna and determine whether is human or mutant.
        /// </summary>
        /// <param name="dnaInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A true/false flag based on result.</returns>
        Task<bool> ProcessDnaSampleAsync(DnaInfo dnaInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Get dnas statistics.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A DnaStatisticsDo result.</returns>
        Task<DnaStatisticsDo> GetDnaStatisticsAsync(CancellationToken cancellationToken);
    }
}
