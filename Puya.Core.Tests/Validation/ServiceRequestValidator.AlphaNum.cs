using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_AlphaNum_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntCode : ServiceRequest
        {
            [AlphaNum]
            public int Code { get; set; }
        }
        public class ModelWithNullableIntCode : ServiceRequest
        {
            [AlphaNum]
            public int? Code { get; set; }
        }
        public class ModelWithStringCode : ServiceRequest
        {
            [AlphaNum]
            public string Code { get; set; }
        }
        [Fact]
        public void value_with_invalid_type1_should_fail()
        {
            ShouldFail(new ModelWithIntCode(), "Code", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void value_with_invalid_type2_should_pass()
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
        public void value_with_alpha_should_pass()
        {
            ShouldPass(new ModelWithStringCode { Code = "ali" });
        }
        [Fact]
        public void value_with_alphanum_should_pass()
        {
            ShouldPass(new ModelWithStringCode { Code = "ali1" });
        }
        [Fact]
        public void value_with_num_should_pass()
        {
            ShouldPass(new ModelWithStringCode { Code = "1" });
        }
        [Fact]
        public void value_with_non_alphanum_should_fail()
        {
            ShouldFail(new ModelWithStringCode { Code = "alireza1+" }, "Code", "NotAlphaNum");
        }
    }
}
