namespace MagnetosBrotherhood.Api.Tests.Controllers
{
    using AutoMapper;
    using MagnetosBrotherhood.Api.Controllers;
    using MagnetosBrotherhood.Api.Models;
    using MagnetosBrotherhood.Domain.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Set of tests for Mutant controller.
    /// </summary>
    [TestClass]
    public class MutantControllerTests
    {
        //Mock instances for services.
        private readonly IMapper _mockMapper;
        private readonly Mock<IMutantService> _mockMutantService;

        // System under test.
        private readonly MutantController _sut;

        /// <summary>
        /// Initialize a new instance of <see cref="MutantControllerTests"/> class.
        /// </summary>
        public MutantControllerTests()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile< Mappers.MappingProfile>();
            });
            _mockMapper = mappingConfig.CreateMapper();
            _mockMutantService = new Mock<IMutantService>();
            _sut = new MutantController(_mockMapper, _mockMutantService.Object);
        }

        /// <summary>
        /// Test that ProcessDnaSample returns a Bad Request response when no input.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessDnaSample_Returns_BadRequest_When_NoInput()
        {
            // arrange
            DnaDto dnaCheckRequest = null;

            // act
            var response = await _sut.ProcessDnaSample(dnaCheckRequest, CancellationToken.None);

            // assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// Test that ProcessDnaSample returns an Ok response when successfull.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessDnaSample_Returns_Ok_When_Successfull()
        {
            // arrange
            var dnaCheckRequest = BuildDnaRequest();
            _mockMutantService.Setup(e => e.ProcessDnaSampleAsync(It.IsAny<Domain.Models.DnaInfo>(), CancellationToken.None)).ReturnsAsync(true);

            // act
            var response = await _sut.ProcessDnaSample(dnaCheckRequest, CancellationToken.None);

            // assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        /// <summary>
        /// Test that ProcessDnaSample returns a Forbidden response when failure.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessDnaSample_Returns_Forbidden_When_Failure()
        {
            // arrange
            var dnaCheckRequest = BuildDnaRequest();
            _mockMutantService.Setup(e => e.ProcessDnaSampleAsync(It.IsAny<Domain.Models.DnaInfo>(), CancellationToken.None)).ReturnsAsync(false);

            // act
            var response = await _sut.ProcessDnaSample(dnaCheckRequest, CancellationToken.None);
            var result = response as StatusCodeResult;

            // assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(result);
            Assert.AreEqual(403, result.StatusCode);
        }

        /// <summary>
        /// Test that GetDnaStatistics returns Not Found when there is no statistics data.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetDnaStatistics_Returns_NoFound_When_NoData()
        {
            // arrange
            _mockMutantService.Setup(e => e.GetDnaStatisticsAsync(CancellationToken.None)).ReturnsAsync((Domain.Models.DnaStatisticsDo)null);

            // act
            var response = await _sut.GetDnaStatistics(CancellationToken.None);

            // assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        /// <summary>
        /// Test that GetDnaStatistics returns a json with statistics data when success.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetDnaStatistics_Returns_OkJson_When_Success()
        {
            // arrange
            var humansCount = 2;
            var mutantsCount = 2;
            var statistics = BuildStatistics(mutantsCount, humansCount);
            _mockMutantService.Setup(e => e.GetDnaStatisticsAsync(CancellationToken.None)).ReturnsAsync(statistics);

            // act
            var response = await _sut.GetDnaStatistics(CancellationToken.None);
            var result = response as OkObjectResult;
            var data = JsonConvert.DeserializeObject<Models.DnaStatistics>(result.Value as string);

            // assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(humansCount, data.HumanDnaCount);
            Assert.AreEqual(mutantsCount, data.MutantDnaCount);
        }

        #region Private Methods

        /// <summary>
        /// Build a new DnaDto request.
        /// </summary>
        /// <returns></returns>
        private DnaDto BuildDnaRequest()
        {
            return new DnaDto
            {
                Dna = new string[] { "test" }
            };
        }

        /// <summary>
        /// Build a new DnaStatisticsDo object.
        /// </summary>
        /// <param name="mutants"></param>
        /// <param name="humans"></param>
        /// <returns></returns>
        private Domain.Models.DnaStatisticsDo BuildStatistics(int mutants, int humans)
        {
            return new Domain.Models.DnaStatisticsDo
            {
                MutantDnaCount = mutants,
                HumanDnaCount = humans
            };
        }

        #endregion
    }
}
