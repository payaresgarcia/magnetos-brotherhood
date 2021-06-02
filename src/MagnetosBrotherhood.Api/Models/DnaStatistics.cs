namespace MagnetosBrotherhood.Api.Models
{
    using Newtonsoft.Json;

    public class DnaStatistics
    {
        [JsonProperty("count_mutant_dna")]
        public long MutantDnaCount { get; set; }

        [JsonProperty("count_human_dna")]
        public long HumanDnaCount { get; set; }

        [JsonProperty("ratio")]
        public double DnaRatio { get; set; }
    }
}
