namespace MagnetosBrotherhood.Domain.Repositories.Tests
{
    using AutoMapper;
    using MagnetosBrotherhood.DAL.Interfaces;
    using MagnetosBrotherhood.Domain.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Set of tests for MutantRepository.
    /// </summary>
    [TestClass]
    public class MutantRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMutantDynamoDb> _mockMutantDynamoDbRepository;
        private readonly MutantRepository _sut;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantRepositoryTests"/> class.
        /// </summary>
        public MutantRepositoryTests()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Domain.Models.DnaEntryDo, DAL.Models.DnaEntry>().ReverseMap();
            });
            _mapper = mappingConfig.CreateMapper();
            _mockMutantDynamoDbRepository = new Mock<IMutantDynamoDb>();
            _sut = new MutantRepository(_mapper, _mockMutantDynamoDbRepository.Object);
        }

        /// <summary>
        /// Test that SaveDnaRequest throws an exception when no input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveDnaRequest_Throws_Exception_When_NoInput()
        {
            // act
            await _sut.SaveDnaRequestAsync(null, CancellationToken.None);
        }

        /// <summary>
        /// Test that SaveDnaRequest process entry without errors when success.
        /// </summary>
        [TestMethod]
        public async Task SaveDnaRequest_Process_Entry_Without_Errors_When_Success()
        {
            // arrange
            var domainEntry = new Domain.Models.DnaEntryDo();
            var dalEntry = new DAL.Models.DnaEntry();
            _mockMutantDynamoDbRepository.Setup(e => e.CreateDnaEntryAsync(dalEntry, CancellationToken.None));

            // act
            await _sut.SaveDnaRequestAsync(domainEntry, CancellationToken.None);
        }

        /// <summary>
        /// Test that GetDnaEntries return an empty list when no data.
        /// </summary>
        [TestMethod]
        public async Task GetDnaEntries_Returns_Empty_List_When_NoData()
        {
            // arrange
            _mockMutantDynamoDbRepository.Setup(e => e.GetDnaEntriesAsync(CancellationToken.None)).ReturnsAsync((IEnumerable<DAL.Models.DnaEntry>)null);

            // act
            var result = await _sut.GetDnaEntriesAsync(CancellationToken.None);

            // assert
            Assert.AreEqual(0, (result as List<DnaEntryDo>).Count);
        }

        /// <summary>
        /// Test that GetDnaEntries return a list of dna entries when data exists.
        /// </summary>
        [TestMethod]
        public async Task GetDnaEntries_Returns_Entries_List_When_Data()
        {
            // arrange
            var entriesList = Enumerable.Range(0, 5).Select(e => CreateDnaEntry(e.ToString())).ToList();
            _mockMutantDynamoDbRepository.Setup(e => e.GetDnaEntriesAsync(CancellationToken.None)).ReturnsAsync(entriesList);

            // act
            var result = await _sut.GetDnaEntriesAsync(CancellationToken.None);

            // assert
            Assert.AreEqual(entriesList?.Count, (result as List<DnaEntryDo>).Count);
        }

        #region

        /// <summary>
        /// Create a new DnaEntry object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private DAL.Models.DnaEntry CreateDnaEntry(string value)
        {
            return new DAL.Models.DnaEntry
            {
                DnaHash = value
            };
        }

        #endregion
    }
}
