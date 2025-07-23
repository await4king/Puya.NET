using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public partial interface IPuyaLogManager :IService<PuyaLogManagerBaseConfig>
    {
        PuyaLogManagerClearBaseAction Clear { get; }
        PuyaLogManagerGetByPKBaseAction GetByPK { get; }
        PuyaLogManagerGetPageBaseAction GetPage { get; }
        PuyaLogManagerDeleteByPKBaseAction DeleteByPK { get; }
    }
}
