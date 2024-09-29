using Puya.CommandLine;
using System;
using System.Diagnostics;

namespace Puya.Extensions
{
    public static class ShellExtensions
    {
        public static ShellExecuteResponse Execute(this ShellHelper shell, string filename, string args, string workingDirectory = "", ProcessWindowStyle? windowStyle = null)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Environment.CurrentDirectory;
            }

            return shell.Execute(new ShellExecuteRequest { FileName = filename, Args = args, WorkingDirectory = workingDirectory, WindowStyle = windowStyle });
        }
    }
}
