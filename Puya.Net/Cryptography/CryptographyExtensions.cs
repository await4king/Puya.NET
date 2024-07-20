using System;
using System.Security.Cryptography;
using Puya.Extensions;
using Puya.Cryptography.v2;
using Puya.Text;

namespace Puya.Cryptography
{
    public static class CryptographyExtensions
    {
        static IBase64Encryption Base64 { get; set; }
        static IEncodingUtility EncodingUtility { get; set; }
        static CryptographyExtensions()
        {
            Base64 = new DotNetBase64Encryption();
            EncodingUtility = new EncodingUtility();
        }
        #region AES
        public static byte[] Encrypt(this IAesEncryption aes, string plainText, byte[] Key, byte[] IV, string cipherMode = "", string paddingMode = "")
        {
            if (!Enum.TryParse(cipherMode, true, out CipherMode _cipherMode))
            {
                _cipherMode = CipherMode.CBC;
            }
            if (!Enum.TryParse(paddingMode, true, out PaddingMode _paddingMode))
            {
                _paddingMode = PaddingMode.PKCS7;
            }

            return aes.Encrypt(plainText, Key, IV, _cipherMode, _paddingMode);
        }
        public static string EncryptToBase64(this IAesEncryption aes, string plainText, byte[] Key, byte[] IV, string cipherMode = "", string paddingMode = "")
        {
            var bytes = Encrypt(aes, plainText, Key, IV, cipherMode, paddingMode);

            return Base64.Encode(bytes);
        }
        public static byte[] Encrypt(this IAesEncryption aes, string plainText, string Key, string IV, string cipherMode = "", string paddingMode = "")
        {
            var key = Base64.Decode(Key);
            var iv = Base64.Decode(IV);

            return aes.Encrypt(plainText, key, iv, cipherMode, paddingMode);
        }
        public static string EncryptToBase64(this IAesEncryption aes, string plainText, string Key, string IV, string cipherMode = "", string paddingMode = "")
        {
            var bytes = Encrypt(aes, plainText, Key, IV, cipherMode, paddingMode);

            return Base64.Encode(bytes);
        }
        public static string Decrypt(this IAesEncryption aes, byte[] cipheredData, byte[] Key, byte[] IV, string cipherMode = "", string paddingMode = "")
        {
            if (!Enum.TryParse(cipherMode, true, out CipherMode _cipherMode))
            {
                _cipherMode = CipherMode.CBC;
            }
            if (!Enum.TryParse(paddingMode, true, out PaddingMode _paddingMode))
            {
                _paddingMode = PaddingMode.PKCS7;
            }

            return aes.Decrypt(cipheredData, Key, IV, _cipherMode, _paddingMode);
        }
        public static string DecryptFromBase64(this IAesEncryption aes, string base64Text, byte[] Key, byte[] IV, string cipherMode = "", string paddingMode = "")
        {
            var data = string.IsNullOrEmpty(base64Text) ? null: Base64.Decode(base64Text);

            return aes.Decrypt(data, Key, IV, cipherMode, paddingMode);
        }
        public static string Decrypt(this IAesEncryption aes, byte[] cipheredData, string Key, string IV, string cipherMode = "", string paddingMode = "")
        {
            var key = Base64.Decode(Key);
            var iv = Base64.Decode(IV);

            return aes.Decrypt(cipheredData, key, iv, cipherMode, paddingMode);
        }
        public static string DecryptFromBase64(this IAesEncryption aes, string base64Text, string Key, string IV, string cipherMode = "", string paddingMode = "")
        {
            var data = string.IsNullOrEmpty(base64Text) ? null : Base64.Decode(base64Text);

            return aes.Decrypt(data, Key, IV, cipherMode, paddingMode);
        }
        #endregion
        #region Base64
        public static string Encode(this IBase64Encryption base64, string data, string encoding)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            var encoder = EncodingUtility.GetEncoding(encoding);
            var source = encoder.GetBytes(data);

            return base64.Encode(source);
        }
        public static string Decode(this IBase64Encryption base64, string data, string encoding)
        {
            var bytes =  base64.Decode(data);
            var encoder = EncodingUtility.GetEncoding(encoding);

            return encoder.GetString(bytes);
        }
        #endregion
        #region HMAC
        public static byte[] Compute(this IHMACEncryption hmac, HMACType type, byte[] data, string key)
        {
            var _key = Base64.Decode(key);

            return hmac.Compute(type, data, _key);
        }
        public static byte[] Compute(this IHMACEncryption hmac, HMACType type, string data, string encoding, byte[] key)
        {
            if (string.IsNullOrEmpty(data))
            {
                return new byte[] { };
            }

            var encoder = EncodingUtility.GetEncoding(encoding);
            var source = encoder.GetBytes(data);

            return hmac.Compute(type, source, key);
        }
        public static byte[] Compute(this IHMACEncryption hmac, HMACType type, string data, string encoding, string key)
        {
            var _key = Base64.Decode(key);

            return hmac.Compute(type, data, encoding, _key);
        }
        public static string ComputeBase64(this IHMACEncryption hmac, HMACType type, string data, string encoding, byte[] key)
        {
            var bytes = hmac.Compute(type, data, encoding, key);

            return Base64.Encode(bytes);
        }
        public static string ComputeBase64(this IHMACEncryption hmac, HMACType type, string data, string encoding, string key)
        {
            var _key = Base64.Decode(key);

            return hmac.ComputeBase64(type, data, encoding, _key);
        }
        public static string ComputeHex(this IHMACEncryption hmac, HMACType type, string data, string encoding, byte[] key, char? separator = null)
        {
            var bytes = hmac.Compute(type, data, encoding, key);

            return bytes.ToHex(separator);
        }
        public static string ComputeHex(this IHMACEncryption hmac, HMACType type, string data, string encoding, string key, char? separator = null)
        {
            var _key = Base64.Decode(key);

            return hmac.ComputeHex(type, data, encoding, _key, separator);
        }
        #endregion
        #region SHA
        public static byte[] Compute(this IShaEncryption hasher, SHAType type, string data, string encoding)
        {
            if (string.IsNullOrEmpty(data))
            {
                return new byte[] { };
            }

            var encoder = EncodingUtility.GetEncoding(encoding);
            var source = encoder.GetBytes(data);

            return hasher.Compute(source, type);
        }
        public static string ComputeBase64(this IShaEncryption hasher, SHAType type, string data, string encoding)
        {
            var bytes = hasher.Compute(type, data, encoding);

            return Base64.Encode(bytes);
        }
        public static string ComputeHex(this IShaEncryption hasher, SHAType type, string data, string encoding, char? separator = null)
        {
            var bytes = hasher.Compute(type, data, encoding);
            
            return bytes.ToHex(separator);
        }
        #endregion
        #region Md5
        public static byte[] Compute(this IMd5Encryption hasher, string data, string encoding)
        {
            if (string.IsNullOrEmpty(data))
            {
                return new byte[] { };
            }

            var encoder = EncodingUtility.GetEncoding(encoding);
            var source = encoder.GetBytes(data);

            return hasher.Compute(source);
        }
        public static string ComputeBase64(this IMd5Encryption hasher, string data, string encoding)
        {
            var bytes = hasher.Compute(data, encoding);

            return Base64.Encode(bytes);
        }
        public static string ComputeHex(this IMd5Encryption hasher, string data, string encoding, char? separator = null)
        {
            var bytes = hasher.Compute(data, encoding);

            return bytes.ToHex(separator);
        }
        #endregion
        #region GetHash
        public static byte[] GetHash(this ICryptographyUtility cryptography, string type, byte[] data, string key)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type");
            }

            if (string.Compare(type, "md5", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return cryptography.MD5.Compute(data);
            }

            if (Enum.TryParse(type, true, out SHAType shaType))
            {
                return cryptography.SHA.Compute(data, shaType);
            }

            if (Enum.TryParse(type, true, out HMACType hmacType))
            {
                return cryptography.HMAC.Compute(hmacType, data, key);
            }

            return new byte[] { };
        }
        public static byte[] GetHash(this ICryptographyUtility cryptography, string type, string data, string encoding, string key)
        {
            var encoder = EncodingUtility.GetEncoding(encoding);
            var bytes = encoder.GetBytes(data);

            return cryptography.GetHash(type, bytes, key);
        }
        public static string GetBase64Hash(this ICryptographyUtility cryptography, string type, string data, string encoding, string key)
        {
            var hash = cryptography.GetHash(type, data, encoding, key);

            return Base64.Encode(hash);
        }
        public static string GetBase64Hash(this ICryptographyUtility cryptography, string type, byte[] data, string key)
        {
            var hash = cryptography.GetHash(type, data, key);

            return Base64.Encode(hash);
        }
        #endregion
    }
}
