using System;
using System.IO;
using System.Security.Cryptography;

namespace CommonLibrary.Encryption
{
    public class EncryptionHelper
    {
        public static bool DecryptFile(string file, byte[] key, byte[] iv)
        {
            throw new NotImplementedException();
        }

        public static T DecryptStream<T>(Stream stream, byte[] key, byte[] iv, Func<string, T> converter)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.Key = key;
            aes.IV = iv;

            string s;

            using (CryptoStream cStream = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cStream))
            {
                s = sr.ReadToEnd();
            }

            return converter(s);
        }

        public static bool EncryptFile(string targetFile, string destinationFile, byte[] key, byte[] iv)
        {
            bool result = true;

            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.Key = key;
                aes.IV = iv;

                byte[] buffer = new byte[1024];

                using (FileStream fsin = new FileStream(targetFile, FileMode.Open, FileAccess.Read))
                using (FileStream fsout = new FileStream(destinationFile, FileMode.Create, FileAccess.ReadWrite))
                using (CryptoStream cstream = new CryptoStream(fsout, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    int read;
                    while ((read = fsin.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cstream.Write(buffer, 0, read);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static Stream EncryptString(string s, byte[] key, byte[] iv)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.Key = key;
            aes.IV = iv;

            MemoryStream ms = new MemoryStream();

            using (CryptoStream cStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (StreamWriter sw = new StreamWriter(cStream))
            {
                sw.Write(s);
            }

            return ms;
        }
    }
}