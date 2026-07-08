using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Phones_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntPhone : ServiceRequest
        {
            [Phones]
            public int Phone { get; set; }
        }
        public class ModelWithNullableIntPhone : ServiceRequest
        {
            [Phones]
            public int? Phone { get; set; }
        }
        public class ModelWithStringPhone : ServiceRequest
        {
            [Phones]
            public string Phone { get; set; }
        }
        public class ModelWithStringPhone1 : ServiceRequest
        {
            [Phones(1)]
            public string Phone { get; set; }
        }
        public class ModelWithStringPhone2 : ServiceRequest
        {
            [Phones(2)]
            public string Phone { get; set; }
        }
        public class ModelWithObjectPhone : ServiceRequest
        {
            [Phones]
            public object Phone { get; set; }
        }
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntPhone(), "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntPhone());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone());
        }
        [Fact]
        public void min_1_null_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone1(), "Phone", "NoItems");
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "" });
        }
        [Fact]
        public void min_1_empty_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone1 { Phone = "" }, "Phone", "NoItems");
        }
        [Fact]
        public void single_phone_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "222-333-4444" });
        }
        [Fact]
        public void min2_single_phone_string_valid_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone2 { Phone = "222-333-4444" }, "Phone", "ItemCountMismatch");
        }
        [Fact]
        public void min2_single_phone_string_invalid_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone2 { Phone = "sfsdf" }, "Phone", "ItemCountMismatch");
        }
        [Fact]
        public void min2_single_phone_string_valid_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "222-333-4444,222-333-5555" });
        }
        [Fact]
        public void single_non_phone_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "a" }, "Phone", "InvalidPhone", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_phone_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringPhone { Phone = "222-333-4444,(555) 555-5555" });
        }
        [Fact]
        public void multiple_non_phone_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "a,b" }, "Phone", "InvalidPhone", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringPhone { Phone = "222-333-4444,b" }, "Phone", "InvalidPhone", null, new { Item = "b", Index = 1 });
        }
        [Fact]
        public void value_with_object_not_string_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = DateTime.Now }, "Phone", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = DateTime.Now }, "Phone", "TypeMismatch");
        }
        [Fact]
        public void single_value_with_object_string_phone_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "222-333-4444" });
        }
        [Fact]
        public void single_value_with_object_string_non_phone_should_not_pass()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "a1" }, "Phone", "InvalidPhone");
        }
        [Fact]
        public void multiple_phone_with_object_value_should_pass()
        {
            ShouldPass(new ModelWithObjectPhone { Phone = "222-333-4444,(555) 555-5555" });
        }
        [Fact]
        public void multiple_non_phone_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "a,b" }, "Phone", "InvalidPhone", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectPhone { Phone = "222-333-4444,b" }, "Phone", "InvalidPhone", null, new { Item = "b", Index = 1 });
        }
    }
}
