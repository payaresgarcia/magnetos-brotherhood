namespace MagnetosBrotherhood.Domain.Common
{
    using System.Text.RegularExpressions;

    public static class DnaOperations
    {
        /// <summary>
        /// Validate that nitrogen base sequence is set properly.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>A bool flag based on valid or not.</returns>
        public static bool IsValidDna(string dnaSequence)
        {
            var regex = new Regex(Constants.Dna.NitrogenBaseRegexPattern);
            var match = regex.Match(dnaSequence);
            return match.Success;
        }

        /// <summary>
        /// Re-structure the dna data to be processed if data is valid.
        /// </summary>
        /// <param name="dnaArray"></param>
        /// <returns>A dna matrix or null based on dna validity.</returns>
        public static char[,] ValidateAndStructureDnaInfo(string[] dnaArray)
        {
            var dnaLength = dnaArray.Length;
            char[,] dnaMatrix = new char[dnaLength, dnaLength];
            for (int i = 0; i < dnaLength; i++)
            {
                if ((dnaLength != dnaArray[i].Length) || !IsValidDna(dnaArray[i]))
                    return null;

                var charArray = dnaArray[i].ToCharArray();
                for (int j = 0; j < charArray.Length; j++)
                {
                    dnaMatrix[i, j] = charArray[j];
                }
            }

            return dnaMatrix;
        }

        /// <summary>
        /// Count for horizontal mutant sequence patterns in dna data.
        /// </summary>
        /// <param name="dnaMatrix"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="dnaSecequenceInfo"></param>
        public static void CountHorizontalMutantSequences(char[,] dnaMatrix, int rows, int cols, DnaSequence dnaSecequenceInfo)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // If it has two mutant sequences at this point, we don't need to go further.
                    if (dnaSecequenceInfo.SequenceCounter > 1)
                        return;

                    //Improve the Big O notation by breaking if we already know that there are less that 4 columns left.
                    if (j + 4 > cols)
                        break;

                    if (dnaMatrix[i, j] == dnaMatrix[i, j + 1] && dnaMatrix[i, j + 1] == dnaMatrix[i, j + 2] && dnaMatrix[i, j + 2] == dnaMatrix[i, j + 3])
                    {
                        dnaSecequenceInfo.Mutex.WaitOne();
                            dnaSecequenceInfo.SequenceCounter++;
                        dnaSecequenceInfo.Mutex.ReleaseMutex();
                    }
                }
            }
        }

        /// <summary>
        /// Count for horizontal mutant sequence patterns in dna data.
        /// </summary>
        /// <param name="dnaMatrix"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="dnaSecequenceInfo"></param>
        public static void CountVerticalMutantSequences(char[,] dnaMatrix, int rows, int cols, DnaSequence dnaSecequenceInfo)
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    // If it has two mutant sequences at this point, we don't need to go further.
                    if (dnaSecequenceInfo.SequenceCounter > 1)
                        return;

                    //Improve the Big O notation by breaking if we already know that there are less that 4 rows left.
                    if (i + 4 > rows)
                        break;

                    if (dnaMatrix[i, j] == dnaMatrix[i + 1, j] && dnaMatrix[i + 1, j] == dnaMatrix[i + 2, j] && dnaMatrix[i + 2, j] == dnaMatrix[i + 3, j])
                    {
                        dnaSecequenceInfo.Mutex.WaitOne();
                            dnaSecequenceInfo.SequenceCounter++;
                        dnaSecequenceInfo.Mutex.ReleaseMutex();
                    }
                }
            }
        }

        /// <summary>
        /// Count for obliques mutant sequence patterns in dna data.
        /// </summary>
        /// <param name="dnaMatrix"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="dnaSecequenceInfo"></param>
        public static void CountObliqueMutantSequences(char[,] dnaMatrix, int rows, int cols, DnaSequence dnaSecequenceInfo)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // If it has two mutant sequences at this point, we don't need to go further.
                    if (dnaSecequenceInfo.SequenceCounter > 1)
                        return;

                    //Improve the Big O notation by breaking if we already know that there are less that 4 rows or columns left.
                    if (j + 4 > cols || i + 4 > rows)
                        break;

                    if (dnaMatrix[i, j] == dnaMatrix[i + 1, j + 1] && dnaMatrix[i + 1, j + 1] == dnaMatrix[i + 2, j + 2] && dnaMatrix[i + 2, j + 2] == dnaMatrix[i + 3, j + 3])
                    {
                        dnaSecequenceInfo.Mutex.WaitOne();
                            dnaSecequenceInfo.SequenceCounter++;
                        dnaSecequenceInfo.Mutex.ReleaseMutex();
                    }
                }
            }
        }
    }
}
