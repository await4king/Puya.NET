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
using System.Collections.Generic;

namespace Puya.Samples.Services.Products
{
	public partial class TapProductsServiceSqlServer : TapProductsServiceBase
    {
        public override TapProductsServiceGetAllBaseAction GetAll { get; protected set; }
        public override TapProductsServiceSaveBaseAction Save { get; protected set; }
		public TapProductsServiceSqlServer(ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger) : base(logger, db, cache, settings, translator, interceptor, logProvider, debugger)
		{
        	GetAll = new TapProductsServiceSqlServerGetAllDefaultAction(this);
        	Actions.Add("GetAll", GetAll);
        	Save = new TapProductsServiceSqlServerSaveDefaultAction(this);
        	Actions.Add("Save", Save);
			Init(logger, db, cache, settings, translator, interceptor, logProvider, debugger);
        }
		partial void Init(ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger);
    }
}

