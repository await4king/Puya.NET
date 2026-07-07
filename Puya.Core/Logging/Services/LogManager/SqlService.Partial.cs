using Puya.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSql : PuyaLogManagerBase
    {
        public IDb Db { get; set; }
        partial void Init(IDb db)
        {
            Db = db;
        }
    }
}
