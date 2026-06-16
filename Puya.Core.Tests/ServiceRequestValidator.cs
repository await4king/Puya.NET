using Puya.Base;
using Puya.Extensions;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Core.Tests
{
    public class Model1: ServiceRequest
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
    public class ServiceRequestValidator_Tests
    {
        void Test_Status(ServiceRequest req, string status)
        {
            var srv = new ServiceRequestValidator();
            var res = new ServiceResponse();
            var oldStatus = res.Status;

            srv.Validate(req, res).Wait();

            Assert.True(res.Status.Equalz(status));
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
        [Fact]
        public void required_attr1()
        {
            var srv = new ServiceRequestValidator();
            var req = new Model2();
            var res = new ServiceResponse();

            var result = srv.Validate(req, res).Result;

            Assert.False(result);
            Assert.True(res.Status.Equalz("InvalidRequest"));
            Assert.True(res.InnerResponses.Count == 1);
            Assert.True(res.InnerResponses[0].Status.Equalz("Required"));
            Assert.True(res.InnerResponses[0].Info.Equalz("Name"));
            Assert.True(res.InnerResponses[0].MessageKey.Equalz("Validation"));
        }
        [Fact]
        public void required_attr2()
        {
            var srv = new ServiceRequestValidator();
            var req = new Model2 { Name = "" };
            var res = new ServiceResponse();

            var result = srv.Validate(req, res).Result;

            Assert.False(result);
            Assert.True(res.Status.Equalz("InvalidRequest"));
            Assert.True(res.InnerResponses.Count == 1);
            Assert.True(res.InnerResponses[0].Status.Equalz("Required"));
            Assert.True(res.InnerResponses[0].Info.Equalz("Name"));
            Assert.True(res.InnerResponses[0].MessageKey.Equalz("Validation"));
        }
        [Fact]
        public void required_attr3()
        {
            var srv = new ServiceRequestValidator();
            var req = new Model2 { Name = "a" };
            var res = new ServiceResponse();
            var oldStatus = res.Status;

            var result = srv.Validate(req, res).Result;

            Assert.True(result);
            Assert.True(res.Status == oldStatus);
        }
        [Fact]
        public void required_attr4()
        {
            var srv = new ServiceRequestValidator();
            var req = new Model3 { Name = "  " };
            var res = new ServiceResponse();

            var result = srv.Validate(req, res).Result;

            Assert.False(result);
            Assert.True(res.Status.Equalz("InvalidRequest"));
            Assert.True(res.InnerResponses.Count == 1);
            Assert.True(res.InnerResponses[0].Status.Equalz("Required"));
            Assert.True(res.InnerResponses[0].Info.Equalz("Name"));
            Assert.True(res.InnerResponses[0].MessageKey.Equalz("Validation"));
        }
    }
}
