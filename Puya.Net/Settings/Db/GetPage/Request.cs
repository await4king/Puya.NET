using Puya.Service;
using Puya.ServiceModel;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Puya.Settings.Service.Models;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsGetPageRequest : ServiceRequest
    {
		public string Key { get; set; }
		public string Value { get; set; }
		public int PageNo { get; set; }
		public int PageSize { get; set; }
		public Puya.Data.CommandParameter RecordCount { get; set; }
		public Puya.Data.CommandParameter PageCount { get; set; }
		public string OrderBy { get; set; }
		public string OrderDir { get; set; }
	}
}
