namespace MagnetosBrotherhood.Domain.Common
{
    using System.Threading;

    public class DnaSequence
    {
        /// <summary>
        /// Mutex to control the increment of the sequence counter across different tasks.
        /// </summary>
        public Mutex Mutex { get; set; }

        /// <summary>
        /// Sequence counter across different tasks.
        /// </summary>
        public int SequenceCounter { get; set; }

        public DnaSequence()
        {
            Mutex = new Mutex();
            SequenceCounter = 0;
        }
    }
}
