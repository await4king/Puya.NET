using System;

namespace Puya.CommandLine
{
    public class ShellExecuteResponse
    {
        public bool Succeeded { get; set; }
        public string Output { get; set; }
        public string Errors { get; set; }
        public string Status { get; set; }
        public int? ExitCode { get; set; }
        public Exception Exception { get; set; }
    }
}
