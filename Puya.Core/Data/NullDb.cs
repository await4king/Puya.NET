using System.Data.Common;

namespace Puya.Data
{
    public class NullDb : DbBase
    {
        public override DbSpecification Specification => new DbSpecification
        {
            Vendor = DbVendor.Unknown,
            Product = DbProduct.Unknown,
            ArchitectureModel = ArchitectureModel.Unknown,
            ConsistencyModel = ConsistencyModel.Unknown,
            DataModel = DataModel.Unknown,
            DataRelationshipModel = DataRelationshipModel.Unknown,
            DbCharacteristics = new DbCharacteristics(),
            DbModelType = DbModelType.Unknown,
            DbUsageType = DbUsageType.Any,
            IndexingModel = IndexingModel.Unknown,
            NoSqlModel = NoSqlModel.None,
            PersistenceModel = PersistenceModel.Unknown,
            QueryCapabilities = new QueryCapabilities(),
            QueryLanguageModel = QueryLanguageModel.Unknown,
            SchemaChangeApproach = SchemaChangeApproach.Unknown,
            SchemaCheckApproach = SchemaCheckApproach.Unknown,
            StorageModel = StorageModel.Unknown,
        };
        protected override DbConnection GetConnectionInternal(string connectionString)
        {
            var result = new FakeDbConnection();

            result.ConnectionString = connectionString;

            return result;
        }
        protected override void SetContextInfo(DbConnection con)
        {
        }
    }
}
