using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MovieCheck.Api.Infra
{
    public static class Util
    {
        public static string HashPassword(string senha)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA1.Create())
            {
                hashBytes = hash.ComputeHash(encoding.GetBytes(senha));
            }

            StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }

            return hashValue.ToString();
        }
        
        //private static bool ValidaEmail(string email)
        //{
        //    if (!Regex.IsMatch(email, "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$"))
        //    {

        //    }
        //}
    }
}
