using Puya.Service;
using System;
using System.Collections.Generic;
using Puya.Logging.Models;
using System.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerGetPageRequest : ServiceRequest
    {
        public TapLogManagerGetPageRequest()
        {
            RecordCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
            PageCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
        }
    }
}
