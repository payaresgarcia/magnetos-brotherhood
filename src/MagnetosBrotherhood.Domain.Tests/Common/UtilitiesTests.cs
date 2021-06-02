namespace MagnetosBrotherhood.Domain.Tests.Common
{
    using MagnetosBrotherhood.Domain.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Text;

    /// <summary>
    /// Set of tests for Utilities.
    /// </summary>
    [TestClass]
    public class UtilitiesTests
    {
        /// <summary>
        /// Test that GetHashBytes returns an empty byte array when no input.
        /// </summary>
        [TestMethod]
        public void GetHashBytes_Returns_Empty_ByteArray_When_NoInput()
        {
            // arrange
            var expected = 0;

            // act
            var result = Utilities.GetHashBytes(null);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(byte[]));
            Assert.AreEqual(expected, result.Length);
        }

        /// <summary>
        /// Test that GetHashBytes returns a byte array based on string input.
        /// </summary>
        [TestMethod]
        public void GetHashBytes_Returns_ByteArray_BasedOn_StringInput()
        {
            // arrange
            var stringText = "Test";

            // act
            var result = Utilities.GetHashBytes(stringText);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(byte[]));
        }

        /// <summary>
        /// Test that ConvertArrayToBase64String returns an empty string when no input.
        /// </summary>
        [TestMethod]
        public void ConvertArrayToBase64String_Returns_Empty_String_When_NoInput()
        {
            // arrange
            var expected = string.Empty;

            // act
            var result = Utilities.ConvertArrayToBase64String(null);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Test that ConvertArrayToBase64String returns a base64 string based on array input.
        /// </summary>
        [TestMethod]
        public void ConvertArrayToBase64String_Returns_Base64String_BasedOn_Input()
        {
            // arrange
            byte[] array = Encoding.ASCII.GetBytes("Test");
            var expected = Convert.ToBase64String(array);

            // act
            var result = Utilities.ConvertArrayToBase64String(array);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}
