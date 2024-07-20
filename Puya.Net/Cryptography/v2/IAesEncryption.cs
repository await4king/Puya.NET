using System.Security.Cryptography;

namespace Puya.Cryptography.v2
{
    public interface IAesEncryption
    {
        byte[] Encrypt(string plainText, byte[] Key, byte[] IV, CipherMode cipherMode, PaddingMode paddingMode);
        string Decrypt(byte[] cipheredData, byte[] Key, byte[] IV, CipherMode cipherMode, PaddingMode paddingMode);
    }
}