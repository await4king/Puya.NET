using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Mobile_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntMobile : ServiceRequest
        {
            [Mobile]
            public int Mobile { get; set; }
        }
        public class ModelWithNullableIntMobile : ServiceRequest
        {
            [Mobile]
            public int? Mobile { get; set; }
        }
        public class ModelWithStringMobile : ServiceRequest
        {
            [Mobile]
            public string Mobile { get; set; }
        }
        public class ModelWithObjectMobile : ServiceRequest
        {
            [Mobile]
            public object Mobile { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntMobile(), "Mobile", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntMobile());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringMobile());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringMobile { Mobile = "" });
        }
        [Fact]
        public void mobile_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringMobile { Mobile = "09123456789" });
        }
        [Fact]
        public void non_mobile_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringMobile { Mobile = "a" }, "Mobile", "InvalidMobile");
        }
        [Fact]
        public void value_with_object_not_string_should_fail()
        {
            ShouldFail(new ModelWithObjectMobile { Mobile = DateTime.Now }, "Mobile", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectMobile { Mobile = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectMobile { Mobile = DateTime.Now }, "Mobile", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_mobile_should_pass()
        {
            ShouldPass(new ModelWithObjectMobile { Mobile = "09123456789" });
        }
        [Fact]
        public void value_with_object_string_non_mobile_should_not_pass()
        {
            ShouldFail(new ModelWithObjectMobile { Mobile = "a1" }, "Mobile", "InvalidMobile");
        }
    }
}
