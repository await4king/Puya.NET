using System.Threading;
using System.Threading.Tasks;
using Puya.Service;
using Puya.Data;
using Puya.Extensions;

namespace Puya.ServiceModel
{
    public abstract class TapBaseDbNonQueryServiceAction<TBaseService, TRequest, TResponse> : TapBaseServiceAction<TBaseService, TRequest, TResponse>
        where TBaseService : TapBaseActionBasedService, IService
        where TRequest : class, ServiceRequest
        where TResponse : ServiceResponse, new()
    {
        public TapBaseDbNonQueryServiceAction(TBaseService owner): base(owner)
        { }
        private async Task DoRun(TRequest request, TResponse response, bool async, CancellationToken cancellation)
        {
            var args = new
            {
                Result = CommandHelper.Result()
            }.Merge(request);

            var sproc = GetSprocName();

            if (async)
            {
                await Db.ExecuteNonQueryCommandAsync(sproc, args, cancellation);
            }
            else
            {
                Db.ExecuteNonQueryCommand(sproc, args);
            }

            response.Finalize(args);
        }
        protected override void RunInternal(TRequest request, TResponse response)
        {
            DoRun(request, response, false, CancellationToken.None).Wait();
        }
        protected override async Task RunInternalAsync(TRequest request, TResponse response, CancellationToken token)
        {
            await DoRun(request, response, true, CancellationToken.None);
        }
    }
}
