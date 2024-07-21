using System.Data.Common;

namespace Puya.Data
{
    public interface IDb
    {
        IConnectionStringProvider ConnectionStringProvider { get; set; }
        DbConnection GetConnection();
        bool PersistConnection { get; set; }
        bool AutoNullEmptyStrings { get; set; }
    }
}
