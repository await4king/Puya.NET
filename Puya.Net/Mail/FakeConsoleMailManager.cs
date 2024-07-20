namespace Puya.Mail
{
    public class FakeConsoleMailConfig : FakeMailConfig
    {
        public bool ColoredOutput { get; set; }
    }
    public class FakeConsoleMailManager : FakeMailManager<FakeConsoleMailConfig>
    {
        protected override void LogInternal(string data)
        {
            System.Console.WriteLine(data);
        }
    }
}
