using Puya.Conversion;
using Puya.Extensions;

namespace Puya.Core.Tests.PuyaExtensions.Object
{
    public class ToDictionary
    {
        [Fact]
        public void Test_ToDictionary_linear_object_no_nesting_and_excluding()
        {
            var obj = new { Name = "ali", Age = 24 };
            var result = obj.ToDictionary();

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Age"));
            Assert.True(result["Name"]?.ToString() == "ali");
            Assert.True(SafeClrConvert.ToInt(result["Age"]) == 24);
        }
        [Fact]
        public void Test_ToDictionary_linear_object_excluding_no_ignoreCase1()
        {
            var obj = new { Name = "ali", Age = 24, City = "tehran", Phone = "1234" };

            var result = obj.ToDictionary("Age,City");

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Phone"));
            Assert.True(!result.ContainsKey("Age"));
            Assert.True(!result.ContainsKey("City"));
            Assert.True(result["Phone"]?.ToString() == "1234");
        }
        [Fact]
        public void Test_ToDictionary_linear_object_excluding_no_ignoreCase2()
        {
            var obj = new { Name = "ali", Age = 24, City = "tehran", Phone = "1234" };

            var result = obj.ToDictionary("Age,city");

            Assert.NotNull(result);
            Assert.True(result.Count == 3);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Phone"));
            Assert.True(result.ContainsKey("City"));
            Assert.True(!result.ContainsKey("Age"));
            Assert.True(result["City"]?.ToString() == "tehran");
        }
        [Fact]
        public void Test_ToDictionary_linear_object_excluding_ignoreCase()
        {
            var obj = new { Name = "ali", Age = 24, City = "tehran", Phone = "1234" };

            var result = obj.ToDictionary("age,city", true);

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Phone"));
            Assert.True(!result.ContainsKey("Age"));
            Assert.True(!result.ContainsKey("City"));
            Assert.True(result["Phone"]?.ToString() == "1234");
        }
        [Fact]
        public void Test_ToDictionary_nested_object_no_nesting_and_excluding()
        {
            var obj = new { Name = "ali", Age = 24, Address = new { City = "tehran", Phone = "1234" } };
            var result = obj.ToDictionary();

            Assert.NotNull(result);
            Assert.True(result.Count == 3);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Age"));
            Assert.True(result.ContainsKey("Address"));
            Assert.True(result["Name"]?.ToString() == "ali");
            Assert.True(SafeClrConvert.ToInt(result["Age"]) == 24);
            Assert.NotNull(result["Address"]);
            Assert.True(!result["Address"].GetType().IsDictionary());
        }
        [Fact]
        public void Test_ToDictionary_nested_object_nesting_but_not_excluding()
        {
            var obj = new { Name = "ali", Age = 24, Address = new { City = "tehran", Phone = "1234" } };
            var result = obj.ToDictionary(true);

            Assert.NotNull(result);
            Assert.True(result.Count == 3);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Age"));
            Assert.True(result.ContainsKey("Address"));
            Assert.True(result["Name"]?.ToString() == "ali");
            Assert.True(SafeClrConvert.ToInt(result["Age"]) == 24);
            Assert.NotNull(result["Address"]);
            Assert.True(result["Address"].GetType().IsDictionary());
        }
        [Fact]
        public void Test_ToDictionary_nested_object_no_nesting_but_excluding_no_ignorecase()
        {
            var obj = new { Name = "ali", Age = 24, Address = new { City = "tehran", Phone = "1234" } };
            var result = obj.ToDictionary("Age,address");

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Address"));
            Assert.True(result["Name"]?.ToString() == "ali");
            Assert.NotNull(result["Address"]);
            Assert.True(!result["Address"].GetType().IsDictionary());
        }
        [Fact]
        public void Test_ToDictionary_nested_object_no_nesting_but_excluding_ignorecase()
        {
            var obj = new { Name = "ali", Age = 24, Address = new { City = "tehran", Phone = "1234" } };
            var result = obj.ToDictionary("Age,address", true);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(!result.ContainsKey("Address"));
            Assert.Null(result["Address"]);
        }
        [Fact]
        public void Test_ToDictionary_nested_object_nesting_but_excluding_no_ignorecase()
        {
            var obj = new { Name = "ali", Age = 24, Address = new { City = "tehran", Phone = "1234" } };
            var result = obj.ToDictionary(true, "Age,address");

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(result.ContainsKey("Address"));
            Assert.True(result["Name"]?.ToString() == "ali");
            Assert.NotNull(result["Address"]);
            Assert.True(result["Address"].GetType().IsDictionary());
        }
        [Fact]
        public void Test_ToDictionary_nested_object_nesting_but_excluding_ignorecase()
        {
            var obj = new { Name = "ali", Age = 24, Address = new { City = "tehran", Phone = "1234" } };
            var result = obj.ToDictionary(true, "Age,address", true);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.ContainsKey("Name"));
            Assert.True(!result.ContainsKey("Address"));
            Assert.Null(result["Address"]);
        }
    }
}
