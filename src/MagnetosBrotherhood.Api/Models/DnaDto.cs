namespace MagnetosBrotherhood.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Dto for handling dna samples.
    /// </summary>
    public class DnaDto
    {
        /// <summary>
        /// Dna data structure.
        /// </summary>
        [Required]
        public string[] Dna { get; set; }
    }
}
