using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_NumericIntInt_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntCode : ServiceRequest
        {
            [NumericInt]
            public int Code { get; set; }
        }
        public class ModelWithFloatCode : ServiceRequest
        {
            [NumericInt]
            public float Code { get; set; }
        }
        public class ModelWithBoolCode : ServiceRequest
        {
            [NumericInt]
            public bool Code { get; set; }
        }
        public class ModelWithNullableIntCode : ServiceRequest
        {
            [NumericInt]
            public int? Code { get; set; }
        }
        public class ModelWithStringCode : ServiceRequest
        {
            [NumericInt]
            public string Code { get; set; }
        }
        public class ModelWithObjectCode : ServiceRequest
        {
            [NumericInt]
            public object Code { get; set; }
        }
        [Fact]
        public void value_with_int_should_pass()
        {
            ShouldPass(new ModelWithIntCode());
        }
        [Fact]
        public void value_with_float_should_fail()
        {
            ShouldFail(new ModelWithFloatCode { Code = 1.23f }, "Code", "NotNumericInt");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntCode());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringCode());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringCode { Code = "" });
        }
        [Fact]
        public void numeric_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringCode { Code = "123" });
        }
        [Fact]
        public void non_numeric_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringCode { Code = "a" }, "Code", "NotNumeric");
        }
        [Fact]
        public void value_with_bool_should_fail()
        {
            ShouldFail(new ModelWithBoolCode { Code = true }, "Code", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_not_numeric_should_fail()
        {
            ShouldFail(new ModelWithObjectCode { Code = DateTime.Now }, "Code", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { Code = "" });
        }
        [Fact]
        public void value_with_object_int_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { Code = 0 });
        }
        [Fact]
        public void value_with_object_string_numeric_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { Code = "1" });
        }
        [Fact]
        public void value_with_object_string_non_numeric_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { Code = "a1" }, "Code", "NotNumeric");
        }
        [Fact]
        public void value_with_object_string_non_integer_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { Code = "1.23" }, "Code", "NotNumericInt");
        }
    }
}
