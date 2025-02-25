using Puya.CommandLine;

namespace Puya.Net.Tests
{
    public class PuyaShell
    {
        [Fact]
        public void TestGitStatus()
        {
            var status = Shell.GitStatus("..");

            Assert.NotNull(status);
        }
    }
}
