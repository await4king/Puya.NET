using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Emails_attribute_Tests: ServiceRequestValidationTests
    {
        #region models
        public class ModelWithIntEmail : ServiceRequest
        {
            [Emails]
            public int Email { get; set; }
        }
        public class ModelWithNullableIntEmail : ServiceRequest
        {
            [Emails]
            public int? Email { get; set; }
        }
        public class ModelWithStringEmail : ServiceRequest
        {
            [Emails]
            public string Email { get; set; }
        }
        public class ModelWithObjectEmail : ServiceRequest
        {
            [Emails]
            public object Email { get; set; }
        }
        #endregion
        [Fact]
        public void value_with_int_should_fail()
        {
            ShouldFail(new ModelWithIntEmail(), "Email", "TypeMismatch");
        }
        [Fact]
        public void value_with_nullable_int_should_pass()
        {
            ShouldPass(new ModelWithNullableIntEmail());
        }
        [Fact]
        public void null_value_should_pass()
        {
            ShouldPass(new ModelWithStringEmail());
        }
        [Fact]
        public void empty_value_should_pass()
        {
            ShouldPass(new ModelWithStringEmail { Email = "" });
        }
        [Fact]
        public void single_email_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringEmail { Email = "a@b.com" });
        }
        [Fact]
        public void single_non_email_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringEmail { Email = "a" }, "Email", "InvalidEmail", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_email_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringEmail { Email = "a@b.com,c@d.com" });
        }
        [Fact]
        public void multiple_non_email_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringEmail { Email = "a,b" }, "Email", "InvalidEmail", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringEmail { Email = "a@b.com,b" }, "Email", "InvalidEmail", null, new { Item = "b", Index = 1 });
        }
        [Fact]
        public void value_with_object_not_string_should_fail()
        {
            ShouldFail(new ModelWithObjectEmail { Email = DateTime.Now }, "Email", "TypeMismatch");
        }
        [Fact]
        public void value_with_object_string_should_pass()
        {
            ShouldPass(new ModelWithObjectEmail { Email = "" });
        }
        [Fact]
        public void value_with_object_int_should_fail()
        {
            ShouldFail(new ModelWithObjectEmail { Email = DateTime.Now }, "Email", "TypeMismatch");
        }
        [Fact]
        public void single_value_with_object_string_email_should_pass()
        {
            ShouldPass(new ModelWithObjectEmail { Email = "a@b.com" });
        }
        [Fact]
        public void single_value_with_object_string_non_email_should_not_pass()
        {
            ShouldFail(new ModelWithObjectEmail { Email = "a1" }, "Email", "InvalidEmail");
        }
        [Fact]
        public void multiple_email_with_object_value_should_pass()
        {
            ShouldPass(new ModelWithObjectEmail { Email = "a@b.com,c@d.com" });
        }
        [Fact]
        public void multiple_non_email_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectEmail { Email = "a,b" }, "Email", "InvalidEmail", new { Item = "a", Index = 0 });
        }
        [Fact]
        public void multiple_mixed_valid_invalid_with_object_value_should_fail()
        {
            ShouldFail(new ModelWithObjectEmail { Email = "a@b.com,b" }, "Email", "InvalidEmail", null, new { Item = "b", Index = 1 });
        }
    }
}
