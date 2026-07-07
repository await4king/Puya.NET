using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_IPv4s_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntIPv4 : ServiceRequest
        {
            [IPv4s]
            public int IPv4 { get; set; }
        }
        public class ModelWithNullableIntIPv4 : ServiceRequest
        {
            [IPv4s]
            public int? IPv4 { get; set; }
        }
        public class ModelWithStringIPv4 : ServiceRequest
        {
            [IPv4s]
            public string IPv4 { get; set; }
        }
        public class ModelWithStringIPv4_2 : ServiceRequest
        {
            [IPv4s(0, 1, true)]
            public string IPv4 { get; set; }
        }
        public class ModelWithObjectIPv4 : ServiceRequest
        {
            [IPv4s]
            public object IPv4 { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntIPv4(), "IPv4", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntIPv4());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringIPv4());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringIPv4 { IPv4 = "" });
        }
        [Fact]
        public void single_ipv4_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringIPv4 { IPv4 = "192.168.10.1" });
        }
        [Fact]
        public void single_non_ipv4_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringIPv4 { IPv4 = "a" }, "IPv4", "InvalidIPv4", new { InvalidItem = "a", Index = 0 });
        }
        [Fact]
        public void multiple_ipv4_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringIPv4 { IPv4 = "192.168.10.1,192.168.255.0" });
        }
        [Fact]
        public void multiple_ipv4_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringIPv4 { IPv4 = "192.168.10.1,192.168.*.*" }, "IPv4", "InvalidIPv4", null, new { InvalidItem = "192.168.*.*", Index = 1 });
        }
        [Fact]
        public void multiple_non_ipv4_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringIPv4 { IPv4 = "a,b" }, "IPv4", "InvalidIPv4", new { InvalidItem = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringIPv4 { IPv4 = "192.168.10.1,b" }, "IPv4", "InvalidIPv4", null, new { InvalidItem = "b", Index = 1 });
        }
        [Fact]
        public void value_with_object_not_string_should_fail()
        {
            ShouldFail(new ModelWithObjectIPv4 { IPv4 = DateTime.Now }, "IPv4", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectIPv4 { IPv4 = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectIPv4 { IPv4 = DateTime.Now }, "IPv4", "TypeMismatch");
        }
        [Fact]
        public void single_value_with_object_string_ipv4_should_pass()
        {
            ShouldPass(new ModelWithObjectIPv4 { IPv4 = "192.168.10.1" });
        }
        [Fact]
        public void single_value_with_object_string_non_ipv4_should_not_pass()
        {
            ShouldFail(new ModelWithObjectIPv4 { IPv4 = "a1" }, "IPv4", "InvalidIPv4");
        }
        [Fact]
        public void multiple_ipv4_with_object_value_should_pass()
        {
            ShouldPass(new ModelWithObjectIPv4 { IPv4 = "192.168.10.1,192.168.10.10" });
        }
        [Fact]
        public void multiple_non_ipv4_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectIPv4 { IPv4 = "a,b" }, "IPv4", "InvalidIPv4", new { InvalidItem = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectIPv4 { IPv4 = "192.168.10.1,b" }, "IPv4", "InvalidIPv4", null, new { InvalidItem = "b", Index = 1 });
        }
    }
}
