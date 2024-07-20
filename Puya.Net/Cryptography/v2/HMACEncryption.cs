using System.Security.Cryptography;

namespace Puya.Cryptography.v2
{
    public class HMACEncryption : IHMACEncryption
    {
        public byte[] Compute(HMACType type, byte[] data, byte[] key)
        {
            HMAC hasher = null;

            if (!(data == null || data.Length == 0))
            {
                switch (type)
                {
                    case HMACType.HMAC:
                        hasher = HMAC.Create();
                        break;
                    case HMACType.HMACMD5:
                        hasher = HMACMD5.Create();
                        break;
                    case HMACType.HMACSHA1:
                        hasher = HMACSHA1.Create();
                        break;
                    case HMACType.HMACSHA256:
                        hasher = HMACSHA256.Create();
                        break;
                    case HMACType.HMACSHA384:
                        hasher = HMACSHA384.Create();
                        break;
                    case HMACType.HMACSHA512:
                        hasher = HMACSHA512.Create();
                        break;
                    case HMACType.HMACRIPEMD160:
                        hasher = HMAC.Create("System.Security.Cryptography.HMACRIPEMD160");
                        break;
                }
            }

            if (hasher != null && key != null && key.Length > 0)
            {
                hasher.Key = key;
            }

            var result = hasher?.ComputeHash(data) ?? new byte[] { };

            return result;
        }
    }
}
