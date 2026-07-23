using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Puya.Service;
using Puya.Data;
using Puya.Collections;
using Puya.Extensions;

namespace Puya.ServiceModel
{
    public abstract class TapBaseDbReaderServiceAction<TBaseService, TRequest, TResponse> : TapBaseServiceAction<TBaseService, TRequest, TResponse>
        where TBaseService : TapBaseActionBasedService, IService
        where TRequest : class, ServiceRequest
        where TResponse : ServiceResponse<IList<DynamicModel>>, new()
    {
        public TapBaseDbReaderServiceAction(TBaseService owner): base(owner)
        { }
        private async Task DoRun(TRequest request, TResponse response, bool async, CancellationToken cancellation)
        {
            var args = new
            {
                Result = CommandHelper.Result()
            }.Merge(request);
            var sproc = GetSprocName();

            response.Data = await Db.ExecuteReaderCommandDynamicAsync(sproc, request, async, cancellation);

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
