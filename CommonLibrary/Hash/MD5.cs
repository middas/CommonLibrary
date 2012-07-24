using System.Security.Cryptography;
using System.Text;
using CommonLibrary.Native;

namespace CommonLibrary.Hash
{
    public class MD5
    {
        public static string GetMD5String(string s)
        {
            StringBuilder sb = new StringBuilder();

            if (!s.IsNullOrEmptyTrim())
            {
                byte[] hash = GetMD5(s);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
            }

            return sb.ToString();
        }

        public static byte[] GetMD5(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] stringBytes = Encoding.ASCII.GetBytes(s);

            return md5.ComputeHash(stringBytes);
        }
    }
}