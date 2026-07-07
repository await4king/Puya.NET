using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Phone_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntPhone : ServiceRequest
        {
            [Phone]
            public int Phone { get; set; }
        }
        public class ModelWithNullableIntPhone : ServiceRequest
        {
            [Phone]
            public int? Phone { get; set; }
        }
        public class ModelWithStringPhone : ServiceRequest
        {
            [Phone]
            public string Phone { get; set; }
        }
        public class ModelWithObjectPhone : ServiceRequest
        {
            [Phone]
            public object Phone { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntPhone(), "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntPhone());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "" });
        }
        [Fact]
        public void phone_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "222-333-4444" });
        }
        [Fact]
        public void non_phone_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "a" }, "Phone", "InvalidPhone");
        }
        [Fact]
        public void value_with_object_not_string_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = DateTime.Now }, "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = DateTime.Now }, "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_phone_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "222-333-4444" });
        }
        [Fact]
        public void value_with_object_string_non_phone_should_not_pass()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "a1" }, "Phone", "InvalidPhone");
        }
    }
}
