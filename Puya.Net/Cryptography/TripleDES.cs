using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.Cryptography
{
    public class TripleDES: IEncryption
    {
        private byte[] GetKeyArray(string key, bool useHasing)
        {
            byte[] keyArray = null;

            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            return keyArray;
        }
        public string Encrypt(string ToEncrypt, string key, bool useHasing)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(ToEncrypt);

            var keyArray = GetKeyArray(key, useHasing);

            var tDes = new TripleDESCryptoServiceProvider();

            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;

            var cTransform = tDes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tDes.Clear();

            var base64 = Convert.ToBase64String(resultArray, 0, resultArray.Length);

            return base64;
        }
        public string Decrypt(string cypherString, string key, bool useHasing)
        {
            var toDecryptArray = Convert.FromBase64String(cypherString);
            var keyArray = GetKeyArray(key, useHasing);
            var tDes = new TripleDESCryptoServiceProvider();

            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;

            var cTransform = tDes.CreateDecryptor();

            try
            {
                var resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                tDes.Clear();

                var result = UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetKey(string n, Encoding e)
        {
            var x = new char[36];
            var y = n.ToByteArray(e).ToBinary();

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = (char)(((i < 10) ? 48 : 87) + i);
            }

            for (byte z = 0; z < y.Length; z++)
            {
                if (y.CharAt(z) == '1')
                {
                    byte i = 0;
                    while (i < x.Length)
                    {
                        byte m = (byte)(i + z + 1);

                        if (m > (x.Length - 1))
                            m = (byte)(m - x.Length);

                        char t = x[i];
                        if (m % 3 == 0)
                        {
                            x[i] = x[m];
                        }
                        else
                        {
                            if (m % 2 == 0)
                            {
                                x[i] = Char.ToUpper(x[m]);
                            }
                            else
                            {
                                x[i] = Char.ToLower(x[m]);
                            }
                        }
                        x[m] = t;

                        i += (byte)(z + 2);
                    }
                }
            }

            return new string(x).Substring(0, 16);
        }

        public string Encrypt(string data, string key)
        {
            return Encrypt(data, key, false);
        }

        public string Decrypt(string data, string key)
        {
            return Decrypt(data, key, false);
        }
    }
}
