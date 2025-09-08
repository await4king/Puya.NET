using System.Security.Claims;

namespace Puya.Core.ServiceModel
{
    public interface IServiceContext
    {
        ClaimsPrincipal User { get; }
    }
}
