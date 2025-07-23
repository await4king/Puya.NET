using Puya.CommandLine;

namespace Puya.Core.Tests
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
