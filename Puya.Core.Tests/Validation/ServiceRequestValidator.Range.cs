using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Range_attribute_Tests : ServiceRequestValidationTests
    {
        #region models
        public class ModelWithNotIntAge : ServiceRequest
        {
            [Range(20, 50)]
            public List<int> Age { get; set; }
        }
        public class ModelWithNullableIntAge : ServiceRequest
        {
            [Range(20, 50)]
            public int? Age { get; set; }
        }
        public class ModelWithIntAge : ServiceRequest
        {
            [Range(20, 50)]
            public int Age { get; set; }
        }
        public class ModelWithStringAge : ServiceRequest
        {
            [Range(20, 50)]
            public string Age { get; set; }
        }
        public class ModelWithObjectAge : ServiceRequest
        {
            [Range(20, 50)]
            public object Age { get; set; }
        }
        #endregion
        #region Age: int
        [Fact]
        public void value_greater_than_max_should_fail()
        {
            ShouldFail(new ModelWithIntAge { Age = 55 }, "Age", "RangeViolation", new { From = 20, To = 50 });
        }
        [Fact]
        public void value_in_range_should_pass()
        {
            ShouldPass(new ModelWithIntAge { Age = 20 });
            ShouldPass(new ModelWithIntAge { Age = 25 });
            ShouldPass(new ModelWithIntAge { Age = 50 });
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
            ShouldFail(new ModelWithNullableIntAge { Age = 53 }, "Age", "RangeViolation", new { From = 20, To = 50 });
        }
        [Fact]
        public void nullable_value_in_range_should_pass()
        {
            ShouldPass(new ModelWithNullableIntAge { Age = 20 });
            ShouldPass(new ModelWithNullableIntAge { Age = 25 });
            ShouldPass(new ModelWithNullableIntAge { Age = 50 });
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
        public void obj_int_value_not_in_range_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = 18 }, "Age", "RangeViolation", new { From = 20, To = 50 });
            ShouldFail(new ModelWithObjectAge { Age = 58 }, "Age", "RangeViolation", new { From = 20, To = 50 });
        }
        [Fact]
        public void obj_value_in_range_should_pass()
        {
            ShouldPass(new ModelWithObjectAge { Age = 20 });
            ShouldPass(new ModelWithObjectAge { Age = 25 });
            ShouldPass(new ModelWithObjectAge { Age = 50 });
        }
        [Fact]
        public void obj_not_numeric_string_value_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = "a12" }, "Age", "NotNumeric");
        }
        [Fact]
        public void obj_string_value_not_in_range_should_fail()
        {
            ShouldFail(new ModelWithObjectAge { Age = "18" }, "Age", "RangeViolation", new { From = 20, To = 50 });
            ShouldFail(new ModelWithObjectAge { Age = "58" }, "Age", "RangeViolation", new { From = 20, To = 50 });
        }
        [Fact]
        public void obj_string_value_in_range_should_pass()
        {
            ShouldPass(new ModelWithObjectAge { Age = "20" });
            ShouldPass(new ModelWithObjectAge { Age = "30" });
            ShouldPass(new ModelWithObjectAge { Age = "50" });
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
        public void string_value_not_in_range_should_fail()
        {
            ShouldFail(new ModelWithStringAge { Age = "18" }, "Age", "RangeViolation", new { From = 20, To = 50 });
            ShouldFail(new ModelWithStringAge { Age = "58" }, "Age", "RangeViolation", new { From = 20, To = 50 });
        }
        [Fact]
        public void string_value_in_range_should_pass()
        {
            ShouldPass(new ModelWithStringAge { Age = "20" });
            ShouldPass(new ModelWithStringAge { Age = "30" });
            ShouldPass(new ModelWithStringAge { Age = "50" });
        }
        #endregion
    }
}
