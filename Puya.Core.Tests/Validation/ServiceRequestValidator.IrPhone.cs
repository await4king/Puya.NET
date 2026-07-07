using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_IrPhone_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntIrPhone : ServiceRequest
        {
            [IrPhone]
            public int Phone { get; set; }
        }
        public class ModelWithNullableIntIrPhone : ServiceRequest
        {
            [IrPhone]
            public int? Phone { get; set; }
        }
        public class ModelWithStringIrPhone : ServiceRequest
        {
            [IrPhone]
            public string Phone { get; set; }
        }
        public class ModelWithObjectIrPhone : ServiceRequest
        {
            [IrPhone]
            public object Phone { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntIrPhone(), "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntIrPhone());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringIrPhone());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringIrPhone { Phone = "" });
        }
        [Fact]
        public void irPhone_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringIrPhone { Phone = "22334455" });
        }
        [Fact]
        public void non_irPhone_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringIrPhone { Phone = "a" }, "Phone", "InvalidPhone");
        }
        [Fact]
        public void value_with_object_not_string_should_fail()
        {
            ShouldFail(new ModelWithObjectIrPhone { Phone = DateTime.Now }, "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectIrPhone { Phone = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectIrPhone { Phone = DateTime.Now }, "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_irPhone_should_pass()
        {
            ShouldPass(new ModelWithObjectIrPhone { Phone = "22334455" });
        }
        [Fact]
        public void value_with_object_string_non_irPhone_should_not_pass()
        {
            ShouldFail(new ModelWithObjectIrPhone { Phone = "a1" }, "Phone", "InvalidPhone");
        }
    }
}
