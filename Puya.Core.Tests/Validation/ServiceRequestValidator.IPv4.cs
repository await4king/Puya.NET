using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_IPv4_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntIPv4 : ServiceRequest
        {
            [IPv4]
            public int IPv4 { get; set; }
        }
        public class ModelWithNullableIntIPv4 : ServiceRequest
        {
            [IPv4]
            public int? IPv4 { get; set; }
        }
        public class ModelWithStringIPv4 : ServiceRequest
        {
            [IPv4]
            public string IPv4 { get; set; }
        }
        public class ModelWithObjectIPv4 : ServiceRequest
        {
            [IPv4]
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
        public void ipv4_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringIPv4 { IPv4 = "192.168.10.1" });
        }
        [Fact]
        public void non_ipv4_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringIPv4 { IPv4 = "a" }, "IPv4", "InvalidIPv4");
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
        public void value_with_object_string_ipv4_should_pass()
        {
            ShouldPass(new ModelWithObjectIPv4 { IPv4 = "192.168.10.1" });
        }
        [Fact]
        public void value_with_object_string_non_ipv4_should_not_pass()
        {
            ShouldFail(new ModelWithObjectIPv4 { IPv4 = "a1" }, "IPv4", "InvalidIPv4");
        }
    }
}
