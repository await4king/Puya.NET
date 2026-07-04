using Puya.Service;

namespace Puya.Core.Tests.Validation
{
    public class ServiceRequestValidator_Base_Tests
    {
        public class Model1 : ServiceRequest
        {
            public string Name { get; set; }
        }
        [Fact]
        public void Model_with_no_validation_attributes()
        {
            var srv = new ServiceRequestValidator();
            var res = new ServiceResponse();
            var oldStatus = res.Status;
            var req = new Model1();

            var result = srv.Validate(req, res).Result;

            Assert.True(result);
            Assert.True(string.Equals(res.Status, oldStatus, StringComparison.Ordinal));
        }
    }
}
