namespace MagnetosBrotherhood.Api.Tests.Controllers
{
    using MagnetosBrotherhood.Api.Controllers;
    using MagnetosBrotherhood.Api.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Set of tests for System controller.
    /// </summary>
    [TestClass]
    public class SystemControllerTests
    {
        // System under test.
        private readonly SystemController _sut;

        /// <summary>
        /// Initialize a new instance of <see cref="SystemControllerTests"/> class.
        /// </summary>
        public SystemControllerTests()
        {
            _sut = new SystemController();
        }

        /// <summary>
        /// Test that Version returns a version message.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Version_Returns_Version()
        {
            // act
            var response = _sut.Version();

            // assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(string));
        }

        /// <summary>
        /// Test that Info returns a message.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Info_Returns_Message()
        {
            // arrange
            var infoDto = new InfoDto
            {
                ApplicantName = "Test"
            };

            // act
            var response = _sut.Info(infoDto);

            // assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(string));
        }
    }
}
