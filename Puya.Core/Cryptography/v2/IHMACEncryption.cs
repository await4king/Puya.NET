namespace Puya.Cryptography.v2
{
    public enum HMACType
    {
        HMAC,
        HMACMD5,
        HMACSHA1,
        HMACSHA256,
        HMACSHA384,
        HMACSHA512,
        HMACRIPEMD160
    }
    public interface IHMACEncryption
    {
        byte[] Compute(HMACType type, byte[] data, byte[] key);
    }
}
