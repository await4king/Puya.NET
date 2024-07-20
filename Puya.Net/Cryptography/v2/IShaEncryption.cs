namespace Puya.Cryptography.v2
{
    public enum SHAType
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
    public interface IShaEncryption
    {
        byte[] Compute(byte[] data, SHAType type);
    }
}
