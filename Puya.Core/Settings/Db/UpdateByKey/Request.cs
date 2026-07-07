using Puya.Service;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsUpdateByKeyRequest : ServiceRequest
    {
		public Puya.Data.CommandParameter Result { get; set; }
		public Puya.Data.CommandParameter Message { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
