using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Mobiles_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntMobile : ServiceRequest
        {
            [Mobiles]
            public int Mobile { get; set; }
        }
        public class ModelWithNullableIntMobile : ServiceRequest
        {
            [Mobiles]
            public int? Mobile { get; set; }
        }
        public class ModelWithStringMobile : ServiceRequest
        {
            [Mobiles]
            public string Mobile { get; set; }
        }
        public class ModelWithObjectMobile : ServiceRequest
        {
            [Mobiles]
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
        public void single_mobile_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringMobile { Mobile = "09123456789" });
        }
        [Fact]
        public void single_non_mobile_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringMobile { Mobile = "a" }, "Mobile", "InvalidMobile", new { InvalidItem = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mobile_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringMobile { Mobile = "09123456789,09103456789" });
        }
        [Fact]
        public void multiple_non_mobile_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringMobile { Mobile = "a,b" }, "Mobile", "InvalidMobile", new { InvalidItem = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringMobile { Mobile = "09123456789,b" }, "Mobile", "InvalidMobile", null, new { InvalidItem = "b", Index = 1 });
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
        public void single_value_with_object_string_mobile_should_pass()
        {
            ShouldPass(new ModelWithObjectMobile { Mobile = "09123456789" });
        }
        [Fact]
        public void single_value_with_object_string_non_mobile_should_not_pass()
        {
            ShouldFail(new ModelWithObjectMobile { Mobile = "a1" }, "Mobile", "InvalidMobile");
        }
        [Fact]
        public void multiple_mobile_with_object_value_should_pass()
        {
            ShouldPass(new ModelWithObjectMobile { Mobile = "09123456789,09103456789" });
        }
        [Fact]
        public void multiple_non_mobile_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectMobile { Mobile = "a,b" }, "Mobile", "InvalidMobile", new { InvalidItem = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectMobile { Mobile = "09123456789,b" }, "Mobile", "InvalidMobile", null, new { InvalidItem = "b", Index = 1 });
        }
    }
}
