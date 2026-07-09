using Puya.Base;
using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Model_Tests: ServiceRequestValidationTests
    {
        public class Model1 : ServiceRequest
        {
            [Required]
            public string Name { get; set; }
            [Range(25,55)]
            public int Age { get; set; }
            [Required]
            public bool? Agree { get; set; }
            [MaxLen(200)]
            public string Address { get; set; }
            [Len(12)]
            public string Passport { get; set; }
            [OneOf("BS,MS,PHD")]
            public string Degree { get; set; }
        }
        [Fact]
        public void test1_should_pass()
        {
            var m = new Model1
            {
                Name = "ali",
                Agree = true,
                Age = 25
            };

            ShouldPass(m);
        }
        [Fact]
        public void test2_should_fail()
        {
            var m = new Model1
            {
            };

            ShouldFail(m, "Name", "Required");
        }
    }
}
