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
	public partial class TapDbSettingsAddRequest : ServiceRequest
    {
		public Puya.Data.CommandParameter Result { get; set; }
		public Puya.Data.CommandParameter Message { get; set; }
		public int AppId { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
