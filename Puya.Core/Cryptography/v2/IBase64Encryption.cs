namespace Puya.Cryptography.v2
{
    public interface IBase64Encryption
    {
        string Encode(byte[] data);
        byte[] Decode(string data);
    }
}