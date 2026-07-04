using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_MaxValue_attribute_Tests : ServiceRequestValidationTests
    {
        #region models
        public class ModelWithNotIntAge : ServiceRequest
        {
            [MaxValue(18)]
            public List<int> Age { get; set; }
        }
        public class ModelWithNullableIntAge : ServiceRequest
        {
            [MaxValue(18)]
            public int? Age { get; set; }
        }
        public class ModelWithIntAge : ServiceRequest
        {
            [MaxValue(18)]
            public int Age { get; set; }
        }
        public class ModelWithStringAge : ServiceRequest
        {
            [MaxValue(18)]
            public string Age { get; set; }
        }
        public class ModelWithObjectAge : ServiceRequest
        {
            [MaxValue(18)]
            public object Age { get; set; }
        }
        #endregion
        #region Age: int
        [Fact]
        public void value_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithIntAge { Age = 23 }, "Age", "ValueTooLarge", new { Max = 18 });
        }
        [Fact]
        public void value_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithIntAge());
            ShouldPass(new ModelWithIntAge { Age = 10 });
        }
        #endregion
        #region Age: int?
        [Fact]
        public void nullable_null_value_should_pass()
        {
            ShouldPass(new ModelWithNullableIntAge());
        }
        [Fact]
        public void nullable_value_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithNullableIntAge { Age = 23 }, "Age", "ValueTooLarge", new { Max = 18 });
        }
        [Fact]
        public void nullable_value_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithNullableIntAge { Age = 10 });
        }
        #endregion
        #region Age: List<int>
        [Fact]
        public void not_int_null_value_should_pass()
        {
            ShouldPass(new ModelWithNotIntAge());
        }
        [Fact]
        public void not_int_value_should_fail1()
        {
            ShouldFail(new ModelWithNotIntAge { Age = new List<int>() }, "Age", "TypeMismatch", new { Expected = "String, Number" });
        }
        #endregion
        #region Age: object
        [Fact]
        public void obj_null_value_should_pass()
        {
            ShouldPass(new ModelWithObjectAge());
        }
        [Fact]
        public void obj_int_value_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = 23 }, "Age", "ValueTooLarge", new { Max = 18 });
        }
        [Fact]
        public void obj_value_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithObjectAge { Age = 0 });
            ShouldPass(new ModelWithObjectAge { Age = 10 });
        }
        [Fact]
        public void obj_not_numeric_string_value_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = "a12" }, "Age", "NotNumeric");
        }
        [Fact]
        public void obj_string_value_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = "23" }, "Age", "ValueTooLarge", new { Max = 18 });
        }
        [Fact]
        public void obj_string_value_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithObjectAge { Age = "10" });
        }
        [Fact]
        public void obj_not_int_value_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = new object() }, "Age", "TypeMismatch", new { Expected = "String, Number" });
        }
        #endregion
        #region Age: string
        [Fact]
        public void null_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringAge());
        }
        [Fact]
        public void empty_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringAge { Age = "" });
        }
        [Fact]
        public void not_numeric_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringAge { Age = "a12" }, "Age", "NotNumeric");
        }
        [Fact]
        public void string_value_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithStringAge { Age = "23" }, "Age", "ValueTooLarge", new { Max = 18 });
        }
        [Fact]
        public void string_value_lower_than_max_should_pass()
        {
            ShouldPass(new ModelWithStringAge { Age = "0" });
            ShouldPass(new ModelWithStringAge { Age = "10" });
        }
        #endregion
    }
}
