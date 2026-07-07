using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_NationalCode_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntCode : ServiceRequest
        {
            [NationalCode]
            public int NationalCode { get; set; }
        }
        public class ModelWithFloatCode : ServiceRequest
        {
            [NationalCode]
            public float NationalCode { get; set; }
        }
        public class ModelWithBoolCode : ServiceRequest
        {
            [NationalCode]
            public bool NationalCode { get; set; }
        }
        public class ModelWithNullableIntCode : ServiceRequest
        {
            [NationalCode]
            public int? NationalCode { get; set; }
        }
        public class ModelWithStringCode : ServiceRequest
        {
            [NationalCode]
            public string NationalCode { get; set; }
        }
        public class ModelWithObjectCode : ServiceRequest
        {
            [NationalCode]
            public object NationalCode { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntCode(), "NationalCode", "TypeMismatch", new { Expected = "String" });
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
            ShouldPass(new ModelWithStringCode { NationalCode = "" });
        }
        [Fact]
        public void numeric_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringCode { NationalCode = "123" });
        }
        [Fact]
        public void non_numeric_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringCode { NationalCode = "a" }, "NationalCode", "NotNationalCode");
        }
        [Fact]
        public void value_with_bool_should_fail()
        {
            ShouldFail(new ModelWithBoolCode { NationalCode = true }, "NationalCode", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_not_numeric_should_fail()
        {
            ShouldFail(new ModelWithObjectCode { NationalCode = DateTime.Now }, "NationalCode", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { NationalCode = "" });
        }
        [Fact]
        public void value_with_object_int_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { NationalCode = 0 });
        }
        [Fact]
        public void value_with_object_string_numeric_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { NationalCode = "1" });
        }
        [Fact]
        public void value_with_object_string_float_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { NationalCode = "1.23" });
        }
        [Fact]
        public void value_with_object_string_non_numeric_should_not_pass()
        {
            ShouldFail(new ModelWithObjectCode { NationalCode = "a1" }, "NationalCode", "NotNationalCode");
        }
    }
}
