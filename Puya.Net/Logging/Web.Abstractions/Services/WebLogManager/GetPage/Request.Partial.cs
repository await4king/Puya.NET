using Puya.Service;
using System;
using System.Collections.Generic;
using Puya.Logging.Web.Abstractions.Models;
using System.Data;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerGetPageRequest : ServiceRequest
    {
        public TapWebLogManagerGetPageRequest()
        {
            RecordCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
            PageCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
        }
    }
}
