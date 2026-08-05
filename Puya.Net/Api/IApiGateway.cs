using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Puya.Api
{
    public interface IApiGateway
    {
        string DefaultApp { get; set; }
        Task<string> ProcessAsync(HttpContext context, CancellationToken cancellation);
    }
}
