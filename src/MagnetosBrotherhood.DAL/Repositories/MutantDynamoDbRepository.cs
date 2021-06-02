namespace MagnetosBrotherhood.DAL.Repositories
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using MagnetosBrotherhood.DAL.Interfaces;
    using MagnetosBrotherhood.DAL.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class MutantDynamoDbRepository : IMutantDynamoDb
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantDynamoDbRepository"/> class.
        /// </summary>
        public MutantDynamoDbRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext ?? throw new ArgumentNullException(nameof(dynamoDbContext));
        }

        /// <summary>
        /// Create a new dna entry in dynamo db table.
        /// </summary>
        /// <param name="dnaEntry"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A new DnaEntry object.</returns>
        public async Task<DnaEntry> CreateDnaEntryAsync(DnaEntry dnaEntry, CancellationToken cancellationToken)
        {
            if (dnaEntry == null)
                throw new ArgumentNullException(nameof(dnaEntry));

            await _dynamoDbContext.SaveAsync(dnaEntry, new DynamoDBOperationConfig { IgnoreNullValues = false }, cancellationToken).ConfigureAwait(false);
            return dnaEntry;
        }

        /// <summary>
        /// Get all dna entries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of dnas</returns>
        public async Task<IEnumerable<DnaEntry>> GetDnaEntriesAsync(CancellationToken cancellationToken)
        {
            var items = new List<DnaEntry>();

            var query = new QueryOperationConfig();
            query.IndexName = DnaEntry.GlobalDnaTypeIndex;
            query.Filter = new QueryFilter();
            query.Filter.AddCondition("recordType", QueryOperator.Equal, new DynamoDBEntry[] { Domain.Constants.Dna.DnaRecordType });
            query.AttributesToGet = new List<string> { "dnaHash", "dnaType", "createdDate" };
            query.Select = SelectValues.SpecificAttributes;

            var response = _dynamoDbContext.FromQueryAsync<DnaEntry>(query, new DynamoDBOperationConfig { IgnoreNullValues = false });
            do
            {
                var resultSet = await response.GetNextSetAsync(cancellationToken).ConfigureAwait(false);
                items.AddRange(resultSet);
            }
            while (!response.IsDone);

            return items;
        }
    }
}
