using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puya.Service;

namespace Puya.ServiceModel
{
    public class TapBaseDbServiceRequest: ServiceRequest
    {
        public Puya.Data.CommandParameter Result { get; set; }
        public Puya.Data.CommandParameter Message { get; set; }
        public TapBaseDbServiceRequest()
        {
            Result = Puya.Data.CommandParameter.Output(SqlDbType.VarChar, "SqlDbType", 50);
            Message = Puya.Data.CommandParameter.Output(SqlDbType.NVarChar, "SqlDbType", 300);
        }
    }
    public class TapBaseDbServicePagingRequest: TapBaseDbServiceRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Puya.Data.CommandParameter RecordCount { get; set; }
        public Puya.Data.CommandParameter PageCount { get; set; }
        public TapBaseDbServicePagingRequest()
        {
            RecordCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
            PageCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
        }
    }
}
