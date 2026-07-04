using Puya.Reflection;
using Puya.Service;
using Puya.Extensions;

namespace Puya.Core.Tests.Validation
{
    public abstract class ServiceRequestValidationTests
    {
        protected void ShouldFail(object request, string propName, string status, object bag = null)
        {
            var srv = new ServiceRequestValidator();
            var res = new ServiceResponse();
            var req = request as ServiceRequest;
            var result = srv.Validate(req, res).Result;

            Assert.False(result);
            Assert.True(res.Status.Equalz("InvalidRequest"));
            Assert.True(res.InnerResponses.Count == 1);
            Assert.True(res.InnerResponses[0].Status.Equalz(status));
            Assert.True(res.InnerResponses[0].Info.Equalz(propName));
            Assert.True(res.InnerResponses[0].MessageKey.Equalz("Validation"));

            if (bag != null)
            {
                var _bag = res.InnerResponses[0].Bag;

                ReflectionHelper.ForEachPublicInstanceReadableProperty(bag.GetType(), prop =>
                {
                    var receivedValue = _bag.GetProp(prop.Name);
                    var expectedValue = prop.GetValue(bag);

                    Assert.Equal(receivedValue?.ToString(), expectedValue?.ToString());
                });
            }
        }
        protected void ShouldPass(object request)
        {
            var srv = new ServiceRequestValidator();
            var req = request as ServiceRequest;
            var res = new ServiceResponse();

            var result = srv.Validate(req, res).Result;

            Assert.True(result);
            Assert.True(!res.Status.Equalz("InvalidRequest"));
            Assert.True(res.InnerResponses.Count == 0);
        }
    }
}
