using Puya.Logging.Web.Abstractions.Services.WebLogManager;
using Puya.Logging.Services.WebLogManager.Clear;
using Puya.Logging.Services.WebLogManager.DeleteByPK;
using Puya.Logging.Services.WebLogManager.GetByPK;
using Puya.Logging.Services.WebLogManager.GetPage;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.WebLogManager
{
	public partial interface IPuyaWebLogManager :IService
    {
        PuyaWebLogManagerClearBaseAction Clear { get; }
        PuyaWebLogManagerGetByPKBaseAction GetByPK { get; }
        PuyaWebLogManagerGetPageBaseAction GetPage { get; }
        PuyaWebLogManagerDeleteByPKBaseAction DeleteByPK { get; }
    }
}
