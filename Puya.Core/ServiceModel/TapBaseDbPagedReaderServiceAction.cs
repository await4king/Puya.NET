using Puya.Collections;
using Puya.Conversion;
using Puya.Data;
using Puya.Extensions;
using Puya.Service;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public abstract class TapBaseDbPagedReaderServiceAction<TBaseService, TRequest, TResponse> : TapBaseServiceAction<TBaseService, TRequest, TResponse>
        where TBaseService : TapBaseActionBasedService, IService
        where TRequest : class, ServiceRequest
        where TResponse : ServiceResponse<PagingResult<DynamicModel>>, new()
    {
        public TapBaseDbPagedReaderServiceAction(TBaseService owner): base(owner)
        { }
        private async Task DoRun(TRequest request, TResponse response, bool async, CancellationToken cancellation)
        {
            var args = new
            {
                Result = CommandHelper.Result(),
                Page = CommandHelper.Page(request),
                PageSize = CommandHelper.PageSize(request),
                RecordCount = CommandHelper.RecordCount(),
                PageCount = CommandHelper.PageCount(),
            }.Merge(request) as IDictionary<string, object>;

            var sproc = GetSprocName();

            response.Data = new PagingResult<DynamicModel>();
            response.Data.Items = await Db.ExecuteReaderCommandDynamicAsync(sproc, args, async, cancellation);

            response.Data.Page = SafeClrConvert.ToInt(args["Page"]);
            response.Data.PageSize = SafeClrConvert.ToInt(args["PageSize"]);
            response.Data.PageCount = SafeClrConvert.ToInt(args["PageCount"]);
            response.Data.RecordCount = SafeClrConvert.ToLong(args["RecordCount"]);

            response.Finalize(args["Result"]);
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
