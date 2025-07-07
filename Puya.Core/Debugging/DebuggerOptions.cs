namespace Puya.Core.Debugging
{
    public class DebuggerOptions
    {
        public bool DebuggingEnabled { get; set; }
        public bool GlobalDebugging { get; set; }
        public string DebuggerUsers { get; set; }
        public string DebuggerRoleName { get; set; }
        public DebuggerOptions()
        {
            DebuggingEnabled = false;
            GlobalDebugging = false;
            DebuggerRoleName = "Debugger";
        }
    }
}
