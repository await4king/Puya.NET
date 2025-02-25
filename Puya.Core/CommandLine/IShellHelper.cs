namespace Puya.CommandLine
{
    public interface IShellHelper
    {
        ShellExecuteResponse Execute(ShellExecuteRequest request);
    }
}
