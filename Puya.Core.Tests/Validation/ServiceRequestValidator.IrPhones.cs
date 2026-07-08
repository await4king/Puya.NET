using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_IrPhones_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntPhone : ServiceRequest
        {
            [IrPhones]
            public int Phone { get; set; }
        }
        public class ModelWithNullableIntPhone : ServiceRequest
        {
            [IrPhones]
            public int? Phone { get; set; }
        }
        public class ModelWithStringPhone : ServiceRequest
        {
            [IrPhones]
            public string Phone { get; set; }
        }
        public class ModelWithObjectPhone : ServiceRequest
        {
            [IrPhones]
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
        public void single_irPhones_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "22334455" });
        }
        [Fact]
        public void single_non_irPhones_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "a" }, "Phone", "InvalidPhone", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_irPhones_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "22334455,021-22334455" });
        }
        [Fact]
        public void multiple_non_irPhones_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "a,b" }, "Phone", "InvalidPhone", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "22334455,b" }, "Phone", "InvalidPhone", null, new { Item = "b", Index = 1 });
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
        public void single_value_with_object_string_irPhones_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "22334455" });
        }
        [Fact]
        public void single_value_with_object_string_non_irPhones_should_not_pass()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "a1" }, "Phone", "InvalidPhone");
        }
        [Fact]
        public void multiple_irPhones_with_object_value_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "22334455,021-22334455" });
        }
        [Fact]
        public void multiple_non_irPhones_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "a,b" }, "Phone", "InvalidPhone", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "22334455,b" }, "Phone", "InvalidPhone", null, new { Item = "b", Index = 1 });
        }
    }
}
