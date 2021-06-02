namespace MagnetosBrotherhood.Domain.Models
{
    public class DnaStatisticsDo
    {
        public long MutantDnaCount { get; set; }
        public long HumanDnaCount { get; set; }
        public double DnaRatio => (double)MutantDnaCount / (double)HumanDnaCount;
    }
}
