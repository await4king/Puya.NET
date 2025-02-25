using System.Diagnostics;

namespace Puya.CommandLine
{
    public class ShellExecuteRequest
    {
        public string FileName { get; set; }
        public string Args { get; set; }
        public string WorkingDirectory { get; set; }
        public ProcessWindowStyle? WindowStyle { get; set; }
    }
}
