using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Numeric_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntCode : ServiceRequest
        {
            [Numeric]
            public int Code { get; set; }
        }
        public class ModelWithFloatCode : ServiceRequest
        {
            [Numeric]
            public float Code { get; set; }
        }
        public class ModelWithBoolCode : ServiceRequest
        {
            [Numeric]
            public bool Code { get; set; }
        }
        public class ModelWithNullableIntCode : ServiceRequest
        {
            [Numeric]
            public int? Code { get; set; }
        }
        public class ModelWithStringCode : ServiceRequest
        {
            [Numeric]
            public string Code { get; set; }
        }
        public class ModelWithObjectCode : ServiceRequest
        {
            [Numeric]
            public object Code { get; set; }
        }
        [Fact]
        public void value_with_int_should_pass()
        {
            ShouldPass(new ModelWithIntCode());
        }
        [Fact]
        public void value_with_float_should_pass()
        {
            ShouldPass(new ModelWithFloatCode());
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
        public void value_with_object_string_float_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { Code = "1.23" });
        }
        [Fact]
        public void value_with_object_string_non_numeric_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { Code = "a1" }, "Code", "NotNumeric");
        }
    }
}
