namespace Puya.Cryptography.v2
{
    public interface IHasher
    {
        byte[] Compute(byte[] data, string type);
    }
}
