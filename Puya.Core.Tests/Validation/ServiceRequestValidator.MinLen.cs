using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_MinLen_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntName : ServiceRequest
        {
            [MinLen(5)]
            public int Name { get; set; }
        }
        public class ModelWithNullableIntName : ServiceRequest
        {
            [MinLen(5)]
            public int? Name { get; set; }
        }
        public class ModelWithStringName : ServiceRequest
        {
            [MinLen(5)]
            public string Name { get; set; }
        }
        [Fact]
        public void value_with_invalid_type1_should_fail()
        {
            ShouldFail(new ModelWithIntName(), "Name", "TypeMismatch", new { Expected = "String" });
        }
        [Fact]
        public void value_with_invalid_type2_should_fail()
        {
            ShouldFail(new ModelWithNullableIntName(), "Name", "LengthTooSmall");
        }
        [Fact]
        public void null_value_should_fail()
        {
            ShouldFail(new ModelWithStringName(), "Name", "LengthTooSmall", new { MinLength = 5, CurrentLength = 0 });
        }
        [Fact]
        public void empty_value_should_fail()
        {
            ShouldFail(new ModelWithStringName { Name = "" }, "Name", "LengthTooSmall", new { MinLength = 5, CurrentLength = 0 });
        }
        [Fact]
        public void value_with_length_lower_than_min_should_fail()
        {
            ShouldFail(new ModelWithStringName { Name = "ali" }, "Name", "LengthTooSmall", new { MinLength = 5, CurrentLength = 3 });
        }
        [Fact]
        public void value_with_length_greater_than_min_should_pass()
        {
            ShouldPass(new ModelWithStringName { Name = "hamidreza" });
        }
    }
}
