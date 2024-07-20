using Puya.Extensions;

namespace Puya.Net.Tests
{
    public class PuyaExtensions
    {
        [Fact]
        public void Test_IterateDictionary()
        {
            var obj = new Dictionary<int, string>
            {
                [10] = "red",
                [20] = "green"
            };

            obj.IterateDictionary((i, item) =>
            {
                var key = (int)item.Key;
                var value = (string)item.Value;

                if (i == 0)
                {
                    Assert.Equal(10, key);
                    Assert.Equal("red", value);
                }
                if (i == 1)
                {
                    Assert.Equal(20, key);
                    Assert.Equal("green", value);
                }
            });
        }
        [Fact]
        public void Test_IterateDictionary2()
        {
            var obj = new Dictionary<int, string>
            {
                [10] = "red",
                [20] = "green"
            };

            var i = 0;

            obj.IterateDictionary((item) =>
            {
                i++;
            });

            Assert.Equal(2, i);
        }
        [Fact]
        public void Test_IterateDictionary3()
        {
            var obj = new Dictionary<int, string>
            {
                [1] = "red",
                [2] = "green",
                [3] = "blue",
                [4] = "white",
                [5] = "black",
            };
            var count = 0;

            obj.IterateDictionary((i, item) =>
            {
                count++;

                return false;
            });

            Assert.Equal(5, count);
        }
        [Fact]
        public void Test_IterateDictionary4()
        {
            var obj = new Dictionary<int, string>
            {
                [1] = "red",
                [2] = "green",
                [3] = "blue",
                [4] = "white",
                [5] = "black",
            };
            var count = 0;

            obj.IterateDictionary((i, item) =>
            {
                count++;

                return i == 2;
            });

            Assert.Equal(3, count);
        }
    }
}
