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
        public void repeating_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringCode { NationalCode = "1111111111" }, "NationalCode", "InvalidNationalCode", new { Reason = "RepeatingDigit" });
        }
        [Fact]
        public void incorrect_length_value_should_fail()
        {
            ShouldFail(new ModelWithStringCode { NationalCode = "123" }, "NationalCode", "InvalidNationalCode", new { Reason = "IncorrectLength" });
            ShouldFail(new ModelWithStringCode { NationalCode = "abc" }, "NationalCode", "InvalidNationalCode", new { Reason = "IncorrectLength" });
        }
        [Fact]
        public void not_numeric_should_fail()
        {
            ShouldFail(new ModelWithStringCode { NationalCode = "abcdefghij" }, "NationalCode", "InvalidNationalCode", new { Reason = "NotNumeric" });
        }
        [Fact]
        public void test_value_should_fail()
        {
            ShouldFail(new ModelWithStringCode { NationalCode = "0123456789" }, "NationalCode", "InvalidNationalCode", new { Reason = "Invalid" });
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectCode { NationalCode = "" });
        }
    }
}
