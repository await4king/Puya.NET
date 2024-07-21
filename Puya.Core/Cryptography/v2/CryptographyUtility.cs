namespace Puya.Cryptography.v2
{
    public class CryptographyUtility : ICryptographyUtility
    {
        /*
         RSA 
            https://gist.github.com/gashupl/27e4de6bd8f021f3d61b3122e6bbf775
            https://developpaper.com/c-rsa-encryption-and-decryption/
         */
        public IAesEncryption AES { get; set; }
        public IBase64Encryption Base64 { get; set; }
        public IHMACEncryption HMAC { get; set; }
        public IMd5Encryption MD5 { get; set; }
        public IShaEncryption SHA { get; set; }
        public IXorEncryption Xor { get; set; }
        public CryptographyUtility()
        {
            AES = new AesEncryption();
            Base64 = new DotNetBase64Encryption();
            HMAC = new HMACEncryption();
            MD5 = new Md5Encryption();
            SHA = new ShaEncryption();
            Xor = new XorEncryption();
        }
    }
}
