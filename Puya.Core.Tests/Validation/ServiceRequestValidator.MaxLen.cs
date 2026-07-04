using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_MaxLen_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntName : ServiceRequest
        {
            [MaxLen(5)]
            public int Name { get; set; }
        }
        public class ModelWithNullableIntName : ServiceRequest
        {
            [MaxLen(5)]
            public int? Name { get; set; }
        }
        public class ModelWithStringName : ServiceRequest
        {
            [MaxLen(5)]
            public string Name { get; set; }
        }
        [Fact]
        public void value_with_invalid_type1_should_fail()
        {
            ShouldFail(new ModelWithIntName(), "Name", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void value_with_invalid_type2_should_pass()
        {
            ShouldPass(new ModelWithNullableIntName());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringName());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringName { Name = "" });
        }
        [Fact]
        public void value_with_length_lower_than_min_should_pass()
        {
            ShouldPass(new ModelWithStringName { Name = "ali" });
        }
        [Fact]
        public void value_with_length_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithStringName { Name = "alireza" }, "Name", "LengthTooLarge", new { MaxLength = 5, CurrentLength = 7 });
        }
    }
}
