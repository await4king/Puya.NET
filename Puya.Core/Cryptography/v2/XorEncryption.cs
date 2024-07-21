using System;

namespace Puya.Cryptography.v2
{
    public class XorEncryption : IXorEncryption
    {
        public string Encode(string data, string key)
        {
            if (string.IsNullOrEmpty(key))
                return data;

            if (string.IsNullOrEmpty(data))
                return string.Empty;

            var result = new char[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (char)(data[i] ^ key[i % key.Length]);
            }

            return new string(result);
        }
        public string Encode(string data, int key)
        {
            string result = "";

            for (int i = 0; i < data.Length; i++)
            {
                var ch = Convert.ToInt32(data[i]);
                ch ^= key;

                result += char.ConvertFromUtf32(ch);
            }

            return result;
        }
    }
}
