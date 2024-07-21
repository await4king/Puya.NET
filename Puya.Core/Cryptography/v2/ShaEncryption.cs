using System.Security.Cryptography;

namespace Puya.Cryptography.v2
{
    public class ShaEncryption : IShaEncryption
    {
        public byte[] Compute(byte[] data, SHAType type)
        {
            HashAlgorithm hasher = null;

            if (!(data == null || data.Length == 0))
            {
                switch (type)
                {
                    case SHAType.SHA1:
                        hasher = SHA1.Create();
                        break;
                    case SHAType.SHA256:
                        hasher = SHA256.Create();
                        break;
                    case SHAType.SHA384:
                        hasher = SHA384.Create();
                        break;
                    case SHAType.SHA512:
                        hasher = SHA512.Create();
                        break;
                }
            }

            var result = hasher?.ComputeHash(data) ?? new byte[] { };

            return result;
        }
    }
}
