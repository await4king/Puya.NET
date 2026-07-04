using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Required_attribute_Tests: ServiceRequestValidationTests
    {
        public class Model1 : ServiceRequest
        {
            public string Name { get; set; }
        }
        public class Model2 : ServiceRequest
        {
            [Required]
            public string Name { get; set; }
        }
        public class Model3 : ServiceRequest
        {
            [Required(IncludeWhiteStrings = true)]
            public string Name { get; set; }
        }
        [Fact]
        public void null_value()
        {
            ShouldFail(new Model2(), "Name", "Required");
        }
        [Fact]
        public void empty_string()
        {
            ShouldFail(new Model2 { Name = "" }, "Name", "Required");
        }
        [Fact]
        public void not_empty_string()
        {
            ShouldPass(new Model2 { Name = "a" });
        }
        [Fact]
        public void whitespace_string()
        {
            ShouldFail(new Model3 { Name = "  " }, "Name", "Required");
        }
    }
}
