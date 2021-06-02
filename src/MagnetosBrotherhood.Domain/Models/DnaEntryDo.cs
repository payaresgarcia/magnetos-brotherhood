namespace MagnetosBrotherhood.Domain.Models
{
    using System;

    /// <summary>
    /// Dna entry info after processing.
    /// </summary>
    public class DnaEntryDo
    {
        /// <summary>
        /// Hashed dna sequence.
        /// </summary>
        public string DnaHash { get; set; }

        /// <summary>
        /// Type of dna (human/mutant).
        /// </summary>
        public string DnaType { get; set; }

        /// <summary>
        /// Datetime where it was entered.
        /// </summary>
        public string CreatedDate { get; set; }
    }
}
