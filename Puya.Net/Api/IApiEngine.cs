using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Puya.Api
{
    public interface IApiEngine
    {
        string DefaultApp { get; set; }
        Task<string> Serve(HttpContext context, CancellationToken cancellation);
    }
}
