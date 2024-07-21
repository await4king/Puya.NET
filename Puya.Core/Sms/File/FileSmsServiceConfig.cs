namespace Puya.Sms
{
    public class FileSmsServiceConfig: SmsConfigItem
    {
        public string FileName { get; set; }
        public string Path { get; set; }

        public override string Type { get { return "file"; } }
    }
}
