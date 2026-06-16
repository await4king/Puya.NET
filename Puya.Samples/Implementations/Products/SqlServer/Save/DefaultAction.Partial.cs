using Puya.Collections;
using Puya.Logging;
using Puya.Data;
using Puya.Caching;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;
using Puya.Translation;
using Puya.Debugging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Conversion;

namespace Puya.Samples.Products.Products
{
    public partial class ProductServiceSqlServerSaveDefaultAction : ProductServiceSaveBaseAction
    {
        private async Task DoRun(ProductServiceSaveRequest request, ProductServiceSaveResponse response, bool async, CancellationToken cancellation)
        {
            try
            {
                do
                {
                    if (request.Id > 0)
                    {
                        if (!string.IsNullOrEmpty(request.Code) && Db.RecordExists("select 1 from Products where Code = @Code and Id <> @Id", new { request.Id, request.Code }))
                        {
                            response.SetStatus("CodeExists");
                            break;
                        }

                        var query = @"
    UPDATE Products
    SET
        Name = CASE WHEN LEN(TRIM(ISNULL(@Name, ''))) = 0 THEN Name ELSE @Name END,
        Code = CASE WHEN LEN(TRIM(ISNULL(@Code, ''))) = 0 THEN Code ELSE @Code END,
        Price = CASE WHEN @Price IS NULL THEN Price ELSE @Price END,
    WHERE Id = @Id";
                        Owner.Debug("query", new { query });

                        await Db.ExecuteNonQuerySqlAsync(query, request, cancellation);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(request.Name))
                        {
                            response.SetStatus("NoName");
                            break;
                        }

                        if (string.IsNullOrWhiteSpace(request.Code))
                        {
                            response.SetStatus("NoCode");
                            break;
                        }

                        if (Db.RecordExists("select 1 from Products where Code = @Code", new { request.Code }))
                        {
                            response.SetStatus("CodeExists");
                            break;
                        }

                        if (!request.Price.HasValue)
                        {
                            response.SetStatus("NoPrice");
                            break;
                        }

                        if (request.Price.Value <= 0)
                        {
                            response.SetStatus("InvalidPrice");
                            break;
                        }
                        var query = @"
    INSERT INTO Products (Name, Code, Price) VALUES (@Name, @Code, @Price)

    SELECT scope_identity()";

                        Owner.Debug("query", new { query });

                        var id = await Db.ExecuteScalarSqlAsync(query, request, cancellation);

                        response.Data = SafeClrConvert.ToInt(id);
                    }

                    response.Succeeded();
                }
                while (false);
            }
            catch (Exception e)
            {
                response.Failed(e);
                Owner.Error(e);
            }
        }
        protected override void RunInternal(ProductServiceSaveRequest request, ProductServiceSaveResponse response)
        {
            DoRun(request, response, false, CancellationToken.None).Wait();
        }
        protected override async Task RunInternalAsync(ProductServiceSaveRequest request, ProductServiceSaveResponse response, CancellationToken cancellation)
        {
            await DoRun(request, response, true, cancellation);
        }
    }
}
