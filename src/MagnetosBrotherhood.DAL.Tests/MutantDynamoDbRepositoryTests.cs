namespace MagnetosBrotherhood.DAL.Tests
{
    using Amazon.DynamoDBv2.DataModel;
    using MagnetosBrotherhood.DAL.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Set of tests for MutantDynamoDbRepository.
    /// </summary>
    [TestClass]
    public class MutantDynamoDbRepositoryTests
    {
        private readonly Mock<IDynamoDBContext> _mockDynamoContext;
        private readonly MutantDynamoDbRepository _sut;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantDynamoDbRepositoryTests"/> class.
        /// </summary>
        public MutantDynamoDbRepositoryTests()
        {
            _mockDynamoContext = new Mock<IDynamoDBContext>();
            _sut = new MutantDynamoDbRepository(_mockDynamoContext.Object);
        }

        /// <summary>
        /// Test that CreateDnaEntry throws an exception when there is no input.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CreateDnaEntry_Throws_Exception_When_NoInput()
        {
            // arrange
            DAL.Models.DnaEntry entry = null;

            // act
            await _sut.CreateDnaEntryAsync(entry, CancellationToken.None);
        }

        [TestMethod]
        public async Task CreateDnaEntry_Returns_Entry_When_Success()
        {
            // arrange
            var entry = new DAL.Models.DnaEntry();
            _mockDynamoContext.Setup(e => e.SaveAsync(entry, new DynamoDBOperationConfig { IgnoreNullValues = false }, CancellationToken.None));

            // act
            await _sut.CreateDnaEntryAsync(entry, CancellationToken.None);

            // assert
            Assert.IsNotNull(entry);
        }
    }
}
