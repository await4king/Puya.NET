using Puya.Extensions;
using System.Diagnostics;

namespace Puya.CommandLine
{
    public static partial class Shell
    {
        public static ShellHelper Instance { get; set; } = new ShellHelper();
        public static ShellExecuteResponse Execute(ShellExecuteRequest request)
        {
            return Instance.Execute(request);
        }
        public static ShellExecuteResponse Execute(string filename, string args, string workingDirectory = "", ProcessWindowStyle? windowStyle = null)
        {
            return Instance.Execute(filename, args, workingDirectory, windowStyle);
        }
    }
}
