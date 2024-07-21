namespace Puya.Cryptography.v2
{
    public interface ICryptographyUtility
    {
        IAesEncryption AES { get; set; }
        IBase64Encryption Base64 { get; set; }
        IHMACEncryption HMAC { get; set; }
        IMd5Encryption MD5 { get; set; }
        IShaEncryption SHA { get; set; }
        IXorEncryption Xor { get; set; }
    }
}