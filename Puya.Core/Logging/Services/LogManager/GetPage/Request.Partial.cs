using Puya.Service;
using System;
using System.Collections.Generic;
using System.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerGetPageRequest : ServiceRequest
    {
        public PuyaLogManagerGetPageRequest()
        {
            RecordCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
            PageCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
        }
    }
}
