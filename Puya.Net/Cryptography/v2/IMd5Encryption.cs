namespace Puya.Cryptography.v2
{
    public interface IMd5Encryption
    {
        byte[] Compute(byte[] data);
    }
}