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
	public partial class ProductServiceRegistration : ServiceRegistery
    {
        public void Build()
        {
			Add(typeof(ProductServiceSqlServerConfig), typeof(ProductServiceSqlServerConfig));
			Add(typeof(ProductServiceBaseConfig), typeof(ProductServiceBaseConfig));
			Add(typeof(ProductServiceBase), typeof(ProductServiceSqlServer));
			Add(typeof(IProductService), typeof(ProductServiceSqlServer));
			Add(typeof(ProductServiceSqlServer), typeof(ProductServiceSqlServer));

			Add(typeof(ProductServiceSaveBaseAction), typeof(ProductServiceSqlServerSaveDefaultAction));
			Add(typeof(ProductServiceGetAllBaseAction), typeof(ProductServiceSqlServerGetAllDefaultAction));
		}
	}
}