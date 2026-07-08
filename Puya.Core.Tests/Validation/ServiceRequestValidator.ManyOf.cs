using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_ManyOf_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntColor : ServiceRequest
        {
            [ManyOf("red,green,blue")]
            public int Color { get; set; }
        }
        public class ModelWithNullableIntColor : ServiceRequest
        {
            [ManyOf("red,green,blue")]
            public int? Color { get; set; }
        }
        public class ModelWithStringColor : ServiceRequest
        {
            [ManyOf("red,green,blue")]
            public string Color { get; set; }
        }
        public class ModelWithStringColor2 : ServiceRequest
        {
            [ManyOf("red,green,blue", 0, 5, true)]
            public string Color { get; set; }
        }
        public class ModelWithObjectColor : ServiceRequest
        {
            [ManyOf("red,green,blue")]
            public object Color { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntColor(), "Color", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntColor());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringColor());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringColor { Color = "" });
        }
        [Fact]
        public void valid_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringColor { Color = "red" });
        }
        [Fact]
        public void valid_string_values_should_pass()
        {
            ShouldPass(new ModelWithStringColor { Color = "red,green" });
            ShouldPass(new ModelWithStringColor { Color = "red,   green" });
            ShouldPass(new ModelWithStringColor { Color = "red,   green  " });
        }
        [Fact]
        public void valid_string_value_should_pass2()
        {
            ShouldPass(new ModelWithStringColor2 { Color = "Red" });
        }
        [Fact]
        public void invalid_string_value_should_fail2()
        {
            ShouldFail(new ModelWithStringColor2 { Color = "Reds" }, "Color", "InvalidItem", new { Allowed = "red,green,blue", Item = "Reds", Index = 0 });
        }
        [Fact]
        public void invalid_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringColor { Color = "a" }, "Color", "InvalidItem", new { Allowed = "red,green,blue", Item = "a", Index = 0 });
            ShouldFail(new ModelWithStringColor { Color = "Red" }, "Color", "InvalidItem", new { Allowed = "red,green,blue", Item = "Red", Index = 0 });
        }
        [Fact]
        public void mixed_valid_and_invalid_values_should_fail()
        {
            ShouldFail(new ModelWithStringColor { Color = "red,green,black" }, "Color", "InvalidItem", new { Allowed = "red,green,blue", Item = "black", Index = 2 });
        }
        [Fact]
        public void value_with_object_not_valid_type_should_fail()
        {
            ShouldFail(new ModelWithObjectColor { Color = DateTime.Now }, "Color", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectColor { Color = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectColor { Color = 0 }, "Color", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_valid_value_should_pass()
        {
            ShouldPass(new ModelWithObjectColor { Color = "red" });
        }
        [Fact]
        public void value_with_object_string_valid_should_pass()
        {
            ShouldPass(new ModelWithObjectColor { Color = "red" });
        }
        [Fact]
        public void value_with_object_string_not_valid_should_not_pass()
        {
            ShouldFail(new ModelWithObjectColor { Color = "a1" }, "Color", "InvalidItem", new { Allowed = "red,green,blue", Item = "a1", Index = 0 });
        }
    }
}
