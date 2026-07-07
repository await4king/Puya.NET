using Puya.Reflection;
using Puya.Service;
using Puya.Extensions;
using Puya.Collections;

namespace Puya.Core.Tests.Validation
{
    public abstract class ServiceRequestValidationTests
    {
        void ValidateBag(object sourceBag, object targetBag)
        {
            if (targetBag != null)
            {
                Assert.True(sourceBag != null);

                var _bag = sourceBag;

                var dynamicBag = targetBag as IDictionary<string, object>;

                if (dynamicBag != null)
                {
                    foreach (var key in dynamicBag.Keys)
                    {
                        var receivedValue = _bag.GetProp(key);
                        var expectedValue = dynamicBag[key];

                        Assert.Equal(receivedValue?.ToString(), expectedValue?.ToString());
                    }
                }
                else
                {
                    ReflectionHelper.ForEachPublicInstanceReadableProperty(targetBag.GetType(), prop =>
                    {
                        var receivedValue = _bag.GetProp(prop.Name);
                        var expectedValue = prop.GetValue(targetBag);

                        Assert.Equal(receivedValue?.ToString(), expectedValue?.ToString());
                    });
                }
            }
        }
        protected void ShouldFail(object request, string propName, string status, params object[] bags)
        {
            var srv = new ServiceRequestValidator();
            var res = new ServiceResponse();
            var req = request as ServiceRequest;
            var result = srv.Validate(req, res).Result;

            Assert.False(result);
            Assert.True(res.Status.Equalz("InvalidRequest"));
            Assert.True(res.InnerResponses.Count == (bags.Length == 0 ? 1: bags.Count(x => x != null)));
            Assert.True(res.InnerResponses[0].Status.Equalz(status));
            Assert.True(res.InnerResponses[0].Info.Equalz(propName));
            Assert.True(res.InnerResponses[0].MessageKey.Equalz("Validation"));

            if (bags?.Length > 0)
            {
                var j = 0;

                for (var i = 0; i < bags.Length; i++)
                {
                    if (bags[i] != null)
                    {
                        ValidateBag(res.InnerResponses[j++].Bag, bags[i]);
                    }
                }
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
