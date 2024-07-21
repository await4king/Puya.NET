namespace Puya.Sms
{
    public class SqlServerSmsServiceConfig: SmsConfigItem
    {
        public string ConnectionString { get; set; }
        public override string Type { get { return "sqlserver"; } }
    }
}
