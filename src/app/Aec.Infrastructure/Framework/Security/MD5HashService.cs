using System;
using System.Security.Cryptography;
using System.Text;

namespace Aec.Infrastructure.Framework.Security
{
    public class MD5HashService : IHashService
    {
        #region Implementation of IHashService

        public string Hash(string clearText)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(clearText));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public bool Verify(string clearText, string hash)
        {
            // Hash the input.
            var hashOfInput = Hash(clearText);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        #endregion
    }
}