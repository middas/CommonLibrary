using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.Hash
{
    public class MD5
    {
        public static byte[] GetMD5(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(s);

            return md5.ComputeHash(stringBytes);
        }

        public static string GetMD5String(string s)
        {
            byte[] hash = GetMD5(s);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}