using Puya.Service;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsDeleteByPKRequest : ServiceRequest
    {
		public Puya.Data.CommandParameter Result { get; set; }
		public Puya.Data.CommandParameter Message { get; set; }
		public int Id { get; set; }
	}
}
