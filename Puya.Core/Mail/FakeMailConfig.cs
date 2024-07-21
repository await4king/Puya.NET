namespace Puya.Mail
{
    public class FakeMailConfig : IMailConfig
    {
        public virtual string DefaultMail { get; set; }
        public virtual bool EnableSSL { get; set; }
        public virtual string Host { get; set; }
        public virtual string Password { get; set; }
        public virtual int Port { get; set; }
        public virtual bool UseDefaultCredentials { get; set; }
        public virtual string Username { get; set; }
    }
}
