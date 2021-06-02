namespace MagnetosBrotherhood.DAL.Models
{
    using Amazon.DynamoDBv2.DataModel;
    using System;

    [DynamoDBTable("magnetos-dna-library", lowerCamelCaseProperties: true)]
    public class DnaEntry
    {
        public const string GlobalDnaTypeIndex = "global_dnaType_index";

        /// <summary>
        /// Hashed dna sequence.
        /// </summary>
        [DynamoDBHashKey]
        public string DnaHash { get; set; }

        /// <summary>
        /// Type of dna (human/mutant).
        /// </summary>
        [DynamoDBRangeKey]
        public string DnaType { get; set; }

        /// <summary>
        /// Datetime where it was entered.
        /// </summary>
        [DynamoDBProperty]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Record for human/mutant valid entries.
        /// </summary>
        [DynamoDBProperty]
        public string RecordType { get; set; }
    }
}
