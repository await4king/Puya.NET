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
using System.Collections.Generic;
using Puya.Security;

namespace Puya.Samples.Products.Products
{
	public partial class ProductServiceSqlServer : ProductServiceBase
    {
        public ProductServiceSqlServerConfig StrongConfig
        {
            get { return Config as ProductServiceSqlServerConfig; }
        }
        public override ProductServiceSaveBaseAction Save { get; protected set; }
        public override ProductServiceGetAllBaseAction GetAll { get; protected set; }
		public ProductServiceSqlServer(ProductServiceSqlServerConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger) : base(config, logger, db, cache, settings, translator, interceptor, logProvider, debugger)
		{
        	Save = new ProductServiceSqlServerSaveDefaultAction(this);
        	Actions.Add("Save", Save);
        	GetAll = new ProductServiceSqlServerGetAllDefaultAction(this);
        	Actions.Add("GetAll", GetAll);
			Init(config, logger, db, cache, settings, translator, interceptor, logProvider, debugger);
        }
		partial void Init(ProductServiceSqlServerConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger);
    }
}

