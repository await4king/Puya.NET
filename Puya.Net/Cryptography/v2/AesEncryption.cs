using System;
using System.IO;
using System.Security.Cryptography;

namespace Puya.Cryptography.v2
{
    public class AesEncryption : IAesEncryption
    {
        // source: https://gist.github.com/RichardHan/0848a25d9466a21f1f38
        public byte[] Encrypt(string plainText, byte[] Key, byte[] IV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] result = null;

            if (!string.IsNullOrEmpty(plainText))
            {
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    aes.Mode = cipherMode;
                    aes.Padding = paddingMode;

                    var enc = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }

                            result = ms.ToArray();
                        }
                    }
                }
            }

            return result ?? new byte[] { };
        }

        public string Decrypt(byte[] cipheredData, byte[] Key, byte[] IV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            string decrypted = null;

            if (cipheredData?.Length > 0)
            {
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    aes.Mode = cipherMode;
                    aes.Padding = paddingMode;

                    var dec = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (var ms = new MemoryStream(cipheredData))
                    {
                        using (var cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                decrypted = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return decrypted;
        }
    }
}
