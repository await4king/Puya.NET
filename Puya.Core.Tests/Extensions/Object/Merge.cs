using Puya.Extensions;

namespace Puya.Core.Tests.Extensions.Object
{
    public class Merge
    {
        [Fact]
        public void Test_Merge1()
        {
            var x = new { a = 10 };
            dynamic y = x.Merge(new { b = 20 });

            Assert.NotNull(y);

            Assert.NotNull(y.a);
            Assert.True(y.a == 10);
            Assert.NotNull(y.b);
            Assert.True(y.b == 20);
        }
    }
}
