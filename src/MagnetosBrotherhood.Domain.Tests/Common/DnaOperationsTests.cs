namespace MagnetosBrotherhood.Domain.Tests.Common
{
    using MagnetosBrotherhood.Domain.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Set of tests for Dna Operations.
    /// </summary>
    [TestClass]
    public class DnaOperationsTests
    {
        /// <summary>
        /// Test that IsValidDna method returns true when the input is a valid nitrogen base sequence.
        /// </summary>
        [TestMethod]
        public void IsValidDna_Returns_True_When_Valid_Input()
        {
            // arrange
            var dnaSequence = BuildDnaSequence();

            // act
            var result = DnaOperations.IsValidDna(dnaSequence);

            // assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test that IsValidDna method returns false when the input is an invalid nitrogen base sequence.
        /// </summary>
        [TestMethod]
        public void IsValidDna_Returns_False_When_InValid_Input()
        {
            // arrange
            var dnaSequence = BuildDnaSequence(isValid: false);

            // act
            var result = DnaOperations.IsValidDna(dnaSequence);

            // assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test that ValidateAndStructureDnaInfo returns a char 2D matrix when passing valid input.
        /// </summary>
        [TestMethod]
        public void ValidateAndStructureDnaInfo_Returns_CharMatrix_When_Valid_Input()
        {
            // arrange
            var dnaSequencesArray = BuildDnaArray();

            // act
            var result = DnaOperations.ValidateAndStructureDnaInfo(dnaSequencesArray);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(char[,]));
            Assert.IsNotNull(result[0,0]);
        }

        /// <summary>
        /// Test that ValidateAndStructureDnaInfo returns null when passing invalid input.
        /// </summary>
        [TestMethod]
        public void ValidateAndStructureDnaInfo_Returns_Null_When_InValid_Input()
        {
            // arrange
            var dnaSequencesArray = BuildDnaArray(isValid: false);

            // act
            var result = DnaOperations.ValidateAndStructureDnaInfo(dnaSequencesArray);

            // assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test that CountHorizontalMutantSequences increments the sequence counter when finds an horizontal mutant pattern.
        /// </summary>
        [TestMethod]
        public void CountHorizontalMutantSequences_Increments_SequenceCounter_When_Horizontal_Mutant_Pattern()
        {
            // arrange
            var dnaMatrix = BuildDnaMatrix("horizontal");
            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            var dnaSecequenceInfo = new DnaSequence();

            // act
            DnaOperations.CountHorizontalMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);

            // assert
            Assert.AreEqual(1, dnaSecequenceInfo.SequenceCounter);
        }

        /// <summary>
        /// Test that CountHorizontalMutantSequences does not increments the sequence counter when not finds an horizontal mutant pattern.
        /// </summary>
        [TestMethod]
        public void CountHorizontalMutantSequences_DoesNot_Increment_SequenceCounter_When_No_Horizontal_Mutant_Pattern()
        {
            // arrange
            var dnaMatrix = BuildDnaMatrix("horizontal", isEmpty: true);
            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            var dnaSecequenceInfo = new DnaSequence();

            // act
            DnaOperations.CountHorizontalMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);

            // assert
            Assert.AreEqual(0, dnaSecequenceInfo.SequenceCounter);
        }

        /// <summary>
        /// Test that CountVerticalMutantSequences increments the sequence counter when finds a vertical mutant pattern.
        /// </summary>
        [TestMethod]
        public void CountVerticalMutantSequences_Increments_SequenceCounter_When_Vertical_Mutant_Pattern()
        {
            // arrange
            var dnaMatrix = BuildDnaMatrix("vertical");
            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            var dnaSecequenceInfo = new DnaSequence();

            // act
            DnaOperations.CountVerticalMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);

            // assert
            Assert.AreEqual(1, dnaSecequenceInfo.SequenceCounter);
        }

        /// <summary>
        /// Test that CountVerticalMutantSequences does not increment the sequence counter when not finds a vertical mutant pattern.
        /// </summary>
        [TestMethod]
        public void CountVerticalMutantSequences_DoesNot_Increments_SequenceCounter_When_NoVertical_Mutant_Pattern()
        {
            // arrange
            var dnaMatrix = BuildDnaMatrix("vertical", isEmpty: true);
            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            var dnaSecequenceInfo = new DnaSequence();

            // act
            DnaOperations.CountVerticalMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);

            // assert
            Assert.AreEqual(0, dnaSecequenceInfo.SequenceCounter);
        }

        /// <summary>
        /// Test that CountObliqueMutantSequences increments the sequence counter when finds a oblique mutant pattern.
        /// </summary>
        [TestMethod]
        public void CountObliqueMutantSequences_Increments_SequenceCounter_When_Vertical_Mutant_Pattern()
        {
            // arrange
            var dnaMatrix = BuildDnaMatrix("oblique");
            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            var dnaSecequenceInfo = new DnaSequence();

            // act
            DnaOperations.CountObliqueMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);

            // assert
            Assert.AreEqual(1, dnaSecequenceInfo.SequenceCounter);
        }

        /// <summary>
        /// Test that CountObliqueMutantSequences does not increments the sequence counter when not finds a oblique mutant pattern.
        /// </summary>
        [TestMethod]
        public void CountObliqueMutantSequences_DoesNot_Increments_SequenceCounter_When_NoVertical_Mutant_Pattern()
        {
            // arrange
            var dnaMatrix = BuildDnaMatrix("oblique", isEmpty: true);
            var rows = dnaMatrix.GetLength(0);
            var cols = dnaMatrix.GetLength(1);
            var dnaSecequenceInfo = new DnaSequence();

            // act
            DnaOperations.CountObliqueMutantSequences(dnaMatrix, rows, cols, dnaSecequenceInfo);

            // assert
            Assert.AreEqual(0, dnaSecequenceInfo.SequenceCounter);
        }

        #region Private Methods

        /// <summary>
        /// Build a dna sequence.
        /// </summary>
        /// <param name="isValid"></param>
        /// <returns>A string sequence</returns>
        private string BuildDnaSequence(bool isValid = true)
        {
            if (isValid)
                return "ATCG";
            else
                return "Invalid";
        }

        /// <summary>
        /// Build a new dna sequences array.
        /// </summary>
        /// <param name="isValid"></param>
        /// <returns>An string array.</returns>
        private string[] BuildDnaArray(bool isValid = true)
        {
            if (isValid)
                return new string[] { "AT", "CG" };
            else
                return new string[]{ "invalid" };
        }

        /// <summary>
        /// Build a new dna matrix.
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="isEmpty"></param>
        /// <returns>A char[,] matrix</returns>
        private char[,] BuildDnaMatrix(string checkType, bool isEmpty = false)
        {
            if (isEmpty)
                return new char[,] { };

            string[] dnaArray;
            switch (checkType)
            {
                case "vertical":
                    dnaArray = new string[] { "ATCG", "AAGT", "ATAT", "AGCA" };
                    break;
                case "oblique":
                    dnaArray = new string[] { "TACG", "CTGT", "ACTG", "AGAT" };
                    break;
                case "horizontal":
                default:
                    dnaArray = new string[] { "CCCC", "CAGT", "TTAT", "AGAA" };
                    break;
            }

            var dnaLength = dnaArray.Length;
            char[,] dnaMatrix = new char[dnaLength, dnaLength];
            for (int i = 0; i < dnaLength; i++)
            {
                var charArray = dnaArray[i].ToCharArray();
                for (int j = 0; j < charArray.Length; j++)
                {
                    dnaMatrix[i, j] = charArray[j];
                }
            }

            return dnaMatrix;
        }

        #endregion
    }
}
