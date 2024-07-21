namespace Puya.Cryptography.v2
{
    public interface IXorEncryption
    {
        string Encode(string data, int key);
        string Encode(string data, string key);
    }
}