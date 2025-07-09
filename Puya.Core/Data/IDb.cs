using Puya.Mapping;
using System.Data.Common;

namespace Puya.Data
{
    public interface IDb
    {
        IConnectionStringProvider ConnectionStringProvider { get; set; }
        IDbContextInfoProvider DbContextInfoProvider { get; set; }
        IMapper Mapper { get; set; }
        DbConnection GetConnection();
        bool PersistConnection { get; set; }
        bool AutoNullEmptyStrings { get; set; }
    }
}
