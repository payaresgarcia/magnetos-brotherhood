namespace MagnetosBrotherhood.Domain.Services.Tests
{
    using MagnetosBrotherhood.Domain.Models;
    using MagnetosBrotherhood.Domain.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Set of tests for Mutant service.
    /// </summary>
    [TestClass]
    public class MutantServiceTests
    {
        private readonly Mock<IMutantRepository> _mockMutantRepository;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantServiceTests"/> class.
        /// </summary>
        public MutantServiceTests()
        {
            _mockMutantRepository = new Mock<IMutantRepository>();
        }

        /// <summary>
        /// Test that ProcessDnaSampleAsync throws exception when passing a null input.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ProcessDnaSampleAsync_Throws_Excetion_When_NoInput()
        {
            // arrange
            var _sut = new MutantServiceMock();

            // act
            _ = await _sut.ProcessDnaSampleAsync(null, CancellationToken.None);
        }

        /// <summary>
        /// Test that ProcessDnaSampleAsync returns false when passing a cancellation token as cancel.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessDnaSampleAsync_Returns_False_When_CancellationToken()
        {
            // arrange
            var _sut = new MutantServiceMock();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var cancellationToken = cancellationTokenSource.Token;
            var dnaInfo = BuildDnaInfo();

            // act
            var result = await _sut.ProcessDnaSampleAsync(dnaInfo, cancellationToken);

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test that ProcessDnaSampleAsync returns false when no mutant found.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessDnaSampleAsync_Returns_False_When_NoMutant()
        {
            // arrange
            var _sut = new MutantServiceMock(returnsTrue: false);
            var dnaInfo = BuildDnaInfo(isValid: false);

            // act
            var result = await _sut.ProcessDnaSampleAsync(dnaInfo, CancellationToken.None);

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test that ProcessDnaSampleAsync returns true when mutant found.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessDnaSampleAsync_Returns_True_When_Mutant()
        {
            // arrange
            var _sut = new MutantServiceMock();
            var dnaInfo = BuildDnaInfo(isValid: false);

            // act
            var result = await _sut.ProcessDnaSampleAsync(dnaInfo, CancellationToken.None);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test that IsMutant returns false when passing a bad input.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task IsMutant_Returns_False_When_Bad_input()
        {
            // arrange
            var _sut = new MutantService(_mockMutantRepository.Object);
            var dnaArray = BuildDnaArray(isValid: false);

            // act
            var result = await _sut.IsMutant(dnaArray);

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test that IsMutant returns true when finds more than one mutant sequence in dna.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task IsMutant_Returns_True_When_More_Than_One_Mutant_Sequence()
        {
            // arrange
            var _sut = new MutantService(_mockMutantRepository.Object);
            var dnaArray = BuildDnaArray(isMutant:true);

            // act
            var result = await _sut.IsMutant(dnaArray);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test that IsMutant returns false when finds less than two mutant sequence in dna.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task IsMutant_Returns_False_When_Less_Than_Two_Mutant_Sequence()
        {
            // arrange
            var _sut = new MutantService(_mockMutantRepository.Object);
            var dnaArray = BuildDnaArray(isMutant: false);

            // act
            var result = await _sut.IsMutant(dnaArray);

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        #region Private Methods

        /// <summary>
        /// Build a dna infomartion object.
        /// </summary>
        /// <param name="isValid"></param>
        /// <returns>A new DnaInfo object.</returns>
        private DnaInfo BuildDnaInfo(bool isValid = true)
        {
            return new DnaInfo
            {
                Dna = isValid ? new string[] { "AT", "CG" } : new string[] { "invalid" }
            };
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
                return new string[] { "invalid" };
        }

        /// <summary>
        /// Build a new dna array.
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="isEmpty"></param>
        /// <returns>A new string array.</returns>
        private string[] BuildDnaArray(bool isMutant, bool isEmpty = false)
        {
            if (isEmpty)
                return new string[] { "invalid" };

            if (isMutant)
                return new string[] { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCATA", "TCACTG" };
            else
                return new string[] { "CTGC", "CAGT", "TTAT", "AGAA" };
        }

        #endregion
    }

    /// <summary>
    /// Mutant service mock for testing.
    /// </summary>
    public class MutantServiceMock : MutantService
    {
        private readonly bool _returnsTrue;

        /// <summary>
        /// Initialize a new instance <see cref="MutantServiceMock"/> of class.
        /// </summary>
        /// <param name="returnsTrue"></param>
        public MutantServiceMock(bool returnsTrue = true): base(new Mock<IMutantRepository>().Object)
        {
            _returnsTrue = returnsTrue;
        }

        /// <summary>
        /// Override IsMutant method behavior for having true or false response based on needs.
        /// </summary>
        /// <param name="dnaArray"></param>
        /// <returns></returns>
        public override async Task<bool> IsMutant(string[] dnaArray)
        {
            return await Task.FromResult(_returnsTrue);
        }
    }
}
