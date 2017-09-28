using System.Text;

namespace CommonLibrary.Hash
{
    public class SHA256
    {
        public static byte[] GetHash(string s)
        {
            System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();

            return sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(s));
        }

        public static byte[] GetHash(string s, byte[] salt)
        {
            System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();

            byte[] b = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] b1 = new byte[b.Length + 16];

            b.CopyTo(b1, 0);

            salt.CopyTo(b1, b.Length);

            return sha.ComputeHash(b1);
        }

        public static string GetHashString(string s)
        {
            byte[] hash = GetHash(s);

            return GetHashString(hash);
        }

        public static string GetHashString(string s, byte[] salt)
        {
            byte[] hash = GetHash(s, salt);

            return GetHashString(hash);
        }

        public static string GetHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}