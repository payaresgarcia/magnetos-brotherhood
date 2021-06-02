namespace MagnetosBrotherhood.DAL.Interfaces
{
    using MagnetosBrotherhood.DAL.Models;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for mutant dynamo db.
    /// </summary>
    public interface IMutantDynamoDb
    {
        /// <summary>
        /// Create a new dna entry in dynamo db table.
        /// </summary>
        /// <param name="dnaEntry"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A new DnaEntry object.</returns>
        Task<DnaEntry> CreateDnaEntryAsync(DnaEntry dnaEntry, CancellationToken cancellationToken);

        /// <summary>
        /// Get all dna entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of dnas</returns>
        Task<IEnumerable<DnaEntry>> GetDnaEntriesAsync(CancellationToken cancellationToken);
    }
}
