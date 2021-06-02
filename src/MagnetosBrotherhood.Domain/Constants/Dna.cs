namespace MagnetosBrotherhood.Domain.Constants
{
    public static class Dna
    {
        public static readonly string NitrogenBaseRegexPattern = "^(A|T|C|G)*$(?-i)";
        public static readonly string HumanDna = "human";
        public static readonly string MutantDna = "mutant";
        public static readonly string DnaRecordType = "dna";
    }
}
