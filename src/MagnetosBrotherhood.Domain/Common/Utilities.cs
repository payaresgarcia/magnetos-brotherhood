namespace MagnetosBrotherhood.Domain.Common
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Utilities
    {
        /// <summary>
        /// Get the hash bytes array from a string.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>A bytes array.</returns>
        public static byte[] GetHashBytes(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return new byte[] { };

            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
        }

        /// <summary>
        /// COnvert an array input into a base64 string representation.
        /// </summary>
        /// <param name="arrayInput"></param>
        /// <returns></returns>
        public static string ConvertArrayToBase64String(byte[] arrayInput)
        {
            if (arrayInput == null)
                return string.Empty;

            return Convert.ToBase64String(arrayInput);
        }
    }
}
