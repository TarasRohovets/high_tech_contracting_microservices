using System;
using System.Security.Cryptography;
using System.Text;

namespace DC.Business.Application.Services.Helpers
{
    internal static class AuthenticationHelper
    {
        public const string AlgorithmMD5 = "md5";
        public const string AlgorithmSHA1 = "sha1";
        public const string AlgorithmSHA256 = "sha256";

        public static string GetHashedPasswordForDatabase(string password, string hashAlgorithm = null)
        {
            if (string.IsNullOrEmpty(password))
                throw new Exception("[AuthenticationHelper][GetHashedPassword]: password can not be null or empty");

            bool isMD5 = !string.IsNullOrEmpty(hashAlgorithm) && hashAlgorithm.ToLower().Equals(AlgorithmMD5);
            bool isSHA1 = !string.IsNullOrEmpty(hashAlgorithm) && hashAlgorithm.ToLower().Equals(AlgorithmSHA1);
            bool isSHA256 = !isMD5 && !isSHA1; // defaults to SHA256

            HashAlgorithm algorithm;

            if (isMD5)
                algorithm = MD5.Create();
            else if (isSHA1)
                algorithm = SHA1.Create();
            else if (isSHA256)
                algorithm = SHA256.Create();
            else
                algorithm = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = algorithm.ComputeHash(bytes);

            string hashedPassword;
            // convert to string as string
            if (isSHA1)
            {
                hashedPassword = Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
            else
            {
                StringBuilder sOutput = new StringBuilder(hashedBytes.Length);
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    sOutput.AppendFormat("{0:x2}", hashedBytes[i]);
                }
                hashedPassword = sOutput.ToString();
            }
            if (algorithm != null)
                try
                {
                    algorithm.Dispose();
                }
                catch (Exception)
                {
                }

            // Return uppercase in case of MD5 just like FormsAuthentication.HashPasswordForStoringInConfigFile(“asdf”, “MD5”)
            if (isMD5)
                return hashedPassword.ToUpper();
            else
                return hashedPassword;


        }
    }
}
