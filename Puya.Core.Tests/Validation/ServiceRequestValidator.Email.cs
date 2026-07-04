using Puya.Base;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Email_attribute_Tests: ServiceRequestValidationTests
    {
        public class ModelWithIntEmail : ServiceRequest
        {
            [Email]
            public int Email { get; set; }
        }
        public class ModelWithNullableIntEmail : ServiceRequest
        {
            [Email]
            public int? Email { get; set; }
        }
        public class ModelWithStringEmail : ServiceRequest
        {
            [Email]
            public string Email { get; set; }
        }
        public class ModelWithObjectEmail : ServiceRequest
        {
            [Email]
            public object Email { get; set; }
        }
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
        public void email_string_value_should_pass()
        {
            ShouldPass(new ModelWithStringEmail { Email = "a@b.com" });
        }
        [Fact]
        public void non_email_string_value_should_fail()
        {
            ShouldFail(new ModelWithStringEmail { Email = "a" }, "Email", "InvalidEmail");
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
        public void value_with_object_string_email_should_pass()
        {
            ShouldPass(new ModelWithObjectEmail { Email = "a@b.com" });
        }
        [Fact]
        public void value_with_object_string_non_email_should_not_pass()
        {
            ShouldFail(new ModelWithObjectEmail { Email = "a1" }, "Email", "InvalidEmail");
        }
    }
}
