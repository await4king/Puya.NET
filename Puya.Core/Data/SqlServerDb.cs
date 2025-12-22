using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Puya.Mapping;

namespace Puya.Data
{
    public class SqlServerDb : DbBase
    {
        public int MaxContextInfoSize { get; set; }
        DbSpecification specs;
        public override DbSpecification Specification => specs;

        public SqlServerDb(): this(null)
        {
        }
        public SqlServerDb(IConnectionStringProvider constrProvider): this(constrProvider, null)
        { }
        public SqlServerDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider) : this(constrProvider, dbContextInfoProvider, null)
        { }
        public SqlServerDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider, IMapper mapper)
        {
            ConnectionStringProvider = constrProvider;
            DbContextInfoProvider = dbContextInfoProvider;
            Mapper = mapper;
            MaxContextInfoSize = 128;

            specs = new DbSpecification
            {
                Vendor = DbVendor.Microsoft,
                Product = DbProduct.SqlServer,
                ArchitectureModel = ArchitectureModel.ClientServer,
                ConsistencyModel = ConsistencyModel.ACID,
                DataModel = DataModel.Relational,
                DataRelationshipModel = DataRelationshipModel.Joins,
                DbCharacteristics = new DbCharacteristics
                {
                    Hosted = true,
                    Hubrid = false,
                    Parallel = true,
                    Polyglot = false
                },
                DbModelType = DbModelType.Relational,
                DbUsageType = DbUsageType.Any,
                IndexingModel = IndexingModel.BTree | IndexingModel.FullText | IndexingModel.Spatial,
                NoSqlModel = NoSqlModel.None,
                PersistenceModel = PersistenceModel.DiskBased,
                QueryCapabilities = new QueryCapabilities
                {
                    SupportsJoins = true,
                    SupportsAggregations = true,
                    SupportsTransactions = true,
                    SupportsFullTextSearch = true,
                    SupportsGeospatialQueries = true,
                    SupportsTemporalQueries = true,
                    SupportsAdHocQueries = true,
                    SupportsWindowFunctions = true
                },
                QueryLanguageModel = QueryLanguageModel.TSql,
                SchemaChangeApproach = SchemaChangeApproach.Fixed,
                SchemaCheckApproach = SchemaCheckApproach.SchemaOnWrite,
                StorageModel = StorageModel.RowOriented,
            };
        }
        protected override void SetContextInfo(DbConnection con)
        {
            if (MaxContextInfoSize > 0)
            {
                var contextInfo = DbContextInfoProvider?.GetContextInfo();

                if (!string.IsNullOrEmpty(contextInfo))
                {
                    var CONTEXT_SQL = $@"
                                  declare @ctx varbinary({MaxContextInfoSize})
                                  set @ctx = cast(@contextinfo as varbinary({MaxContextInfoSize}))
                                  set context_info @ctx";

                    var cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = CONTEXT_SQL;

                    var p = new SqlParameter("@contextinfo", SqlDbType.NVarChar, MaxContextInfoSize);

                    p.Value = contextInfo;

                    cmd.Parameters.Add(p);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        protected override DbConnection GetConnectionInternal(string conenctionString)
        {
            return new SqlConnection(conenctionString);
        }
    }
}
