namespace MagnetosBrotherhood.Domain.Repositories
{
    using MagnetosBrotherhood.Domain.Models;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for mutant repository.
    /// </summary>
    public interface IMutantRepository
    {
        /// <summary>
        /// Save a sequence of dna and for a human or mutant.
        /// </summary>
        /// <param name="dnaEntry"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task object.</returns>
        Task SaveDnaRequestAsync(DnaEntryDo dnaEntry, CancellationToken cancellationToken);

        /// <summary>
        /// Get all dna entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of dnas</returns>
        Task<IEnumerable<DnaEntryDo>> GetDnaEntriesAsync(CancellationToken cancellationToken);
    }
}
