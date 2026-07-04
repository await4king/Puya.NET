using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_NotZero_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntCode : ServiceRequest
        {
            [NotZero]
            public int Code { get; set; }
        }
        public class ModelWithFloatCode : ServiceRequest
        {
            [NotZero]
            public float Code { get; set; }
        }
        public class ModelWithBoolCode : ServiceRequest
        {
            [NotZero]
            public bool Code { get; set; }
        }
        public class ModelWithNullableIntCode : ServiceRequest
        {
            [NotZero]
            public int? Code { get; set; }
        }
        public class ModelWithStringCode : ServiceRequest
        {
            [NotZero]
            public string Code { get; set; }
        }
        public class ModelWithObjectCode : ServiceRequest
        {
            [NotZero]
            public object Code { get; set; }
        }
        [Fact]
        public void value_with_zero_int_should_fail()
        {
            ShouldFail(new ModelWithIntCode(), "Code", "IsZero");
        }
        [Fact]
        public void value_with_non_zero_int_should_pass()
        {
            ShouldPass(new ModelWithIntCode { Code = -10 });
            ShouldPass(new ModelWithIntCode { Code = 10 });
        }
        [Fact]
        public void value_with_zero_float_should_not_pass()
        {
            ShouldFail(new ModelWithFloatCode(), "Code", "IsZero");
        }
        [Fact]
        public void value_with_non_zero_float_should_pass()
        {
            ShouldPass(new ModelWithFloatCode { Code = -10 });
            ShouldPass(new ModelWithFloatCode { Code = 10 });
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntCode());
        }
        [Fact]
        public void value_with_nullable_zero_int_should_not_pass()
        {
            ShouldFail(new ModelWithNullableIntCode { Code = 0 }, "Code", "IsZero");
        }
        [Fact]
        public void value_with_nullable_nonzero_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntCode { Code = 10 });
            ShouldPass(new ModelWithNullableIntCode { Code = -10 });
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
        public void numeric_string_non_zero_value_should_pass()
        {
            ShouldPass(new ModelWithStringCode { Code = "10" });
            ShouldPass(new ModelWithStringCode { Code = "-10" });
        }
        [Fact]
        public void numeric_string_zero_value_should_not_pass()
        {
            ShouldFail(new ModelWithStringCode { Code = "0" }, "Code", "IsZero");
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
        public void value_with_object_zero_int_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { Code = 0 }, "Code", "IsZero");
        }
        [Fact]
        public void value_with_object_string_numeric_non_zero_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { Code = "10" });
            ShouldPass(new ModelWithObjectCode { Code = "-10" });
        }
        [Fact]
        public void value_with_object_string_float_non_zero_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { Code = "1.23" });
        }
        [Fact]
        public void value_with_object_string_non_numeric_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { Code = "a1" }, "Code", "NotNumeric");
        }
        [Fact]
        public void value_with_object_string_zero_numeric_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { Code = "0" }, "Code", "IsZero");
        }
    }
}
