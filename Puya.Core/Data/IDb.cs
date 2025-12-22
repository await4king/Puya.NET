using Puya.Mapping;
using System;
using System.Data.Common;

namespace Puya.Data
{
    [Flags]
    public enum DataModel
    {
        Unknown,
        Relational,     // (tables/rows)
        Document,       // (JSON/BSON): MongoDB, CouchDB, Firebase Firestore
        KeyValue,       // (simple pairs): Redis, DynamoDB, etcd, Riak
        ColumnFamily,   // (column-oriented): Cassandra, HBase, ScyllaDB
        Graph,          // (nodes/edges): Neo4j, Amazon Neptune, ArangoDB
        TimeSeries,     // (timestamped): InfluxDB, TimescaleDB, Prometheus
        Vector,         // (embeddings)
        ObjectOriented, // (objects)
        Hierarchical,   // (tree structures)
        Network,        // (CODASYL)
    }
    [Flags]
    public enum ArchitectureModel
    {
        Unknown,
        ClientServer,   // Most traditional databases
        PeerToPeer,     // Cassandra, Dynamo
        MasterSlave,    // MySQL replication
        MultiMaster,    // CockroachDB, Galera
        SharedNothing,
        SharedDisk,
        Embeded,
        Distributed,
        Serverless,     // FaunaDB, DynamoDB on-demand
    }
    [Flags]
    public enum NoSqlModel
    {
        None,           // Not a NoSQL database
        KeyValue,       // Redis, DynamoDB, etcd, Riak
        DocumentBased,  // MongoDB, CouchDB, Firebase Firestore
        ColumnFamily,   // Cassandra, HBase, ScyllaDB
        Graph,          // Neo4j, Amazon Neptune, ArangoDB
        TimeSeries,     // InfluxDB, TimescaleDB, Prometheus
    }
    [Flags]
    public enum ConsistencyModel
    {
        Unknown,
        Strong,         // ACID
        Eventual,       // BASE
        Causal,         // casualily preserved
        Session,        // per client consistency
        ReadYourWrites, // RYW
        MonotonicReads,
        MonotonicWrites,
        BoundedStaleness,
        ACID = Strong,
        BASE = Eventual,
        RYW = ReadYourWrites
    }
    [Flags]
    public enum SchemaChangeApproach
    {
        Unknown,
        Fixed,      // RDBMS
        Flexible,   // Document DBs
        SchemaLess, // Key-Value, Graph DBs, NoSQL
        Evolving,    // Supports schema changes over time
    }
    [Flags]
    public enum SchemaCheckApproach
    {
        Unknown,
        SchemaOnRead,  // Apply schema when reading data
        SchemaOnWrite,  // Apply schema when writing data
        Schemaless, // No predefined schema
        SchemaMulti // multiple schemas
    }
    [Flags]
    public enum DataRelationshipModel
    {
        Unknown,
        Joins,
        References,
        GraphEdges,
        HierarchicalNesting
    }
    [Flags]
    public enum PersistenceModel
    {
        Unknown,
        Traditional,    // Oracle, MySQL, PostgreSQL, SQL Server
        InMemory,       // Redis, MemSQL, SAP HANA, VoltDB
        DiskBased,
        Hybrid,
        Embeded,        // SQLite, H2, Derby, LevelDB, RocksDB, Berkeley DB
        Distributed,    // CockroachDB, YugabyteDB, TiDB, Cassandra, DynamoDB
        CloudBased,     // AWS Aurora, Azure Cosmos DB, Google Cloud Spanner
    }
    [Flags]
    public enum IndexingModel
    {
        Unknown,
        BTree,
        LSMTree,
        Hash,
        FullText,
        Spatial,
        Vector,
        Composite,
    }
    [Flags]
    public enum StorageModel
    {
        Unknown,
        RowOriented,        // Traditional RDBMS, OLTP optimized
        LogStructured,      // (append-only)
        ColumnOriented,     // OLAP optimized, Analytical databases (Vertica, ClickHouse)
        DocumentOriented,   // MongoDB, CouchDB
        GraphOriented,      // Neo4j
        KeyValueOriented,   // Redis, DynamoDB
    }
    [Flags]
    public enum QueryLanguageModel
    {
        Unknown,
        TSql,       // Traditional SQL
        SqlExtended,
        ApiBased,
        GraphQl,
        NewSql,     // Google Spanner, VoltDB, NuoDB
        RdfTripple,     // AllegroGraph, Virtuoso, Stardog
    }
    public class QueryCapabilities
    {
        public bool SupportsJoins { get; set; }
        public bool SupportsAggregations { get; set; }
        public bool SupportsTransactions { get; set; }
        public bool SupportsFullTextSearch { get; set; }
        public bool SupportsGeospatialQueries { get; set; }
        public bool SupportsTemporalQueries { get; set; }
        public bool SupportsAdHocQueries { get; set; }
        public bool SupportsWindowFunctions { get; set; }
    }
    public enum DbVendor
    {
        Unknown,
        Microsoft,
        Apache,
        Amazon,
        Google,
        Oracle,
        IBM,
        OpenSource
    }
    public enum DbProduct
    {
        Unknown,
        OleDb,
        MicrosoftAccess,
        SqlServer,
        MySql,
        Postgre,
        Oracle,
        Sqlite,
        Db2,
        SapHana,
        Teradata,
        MariaDb,
        AmazonRds,
        AzureSqlDatabase,
        GoogleCloudSql,
        AmazonAurora,
        OracleCloud,
        Firebird,
        Hsqldb,
        H2,
        ApacheDerby,
        Informix,
        Ingres,
        CockroachDb,
        TiDb,
        YugabyteDb,
        MongoDb,
        CouchDb,
        Couchbase,
        Firestore,
        CosmosDB,
        RavenDB,
        Redis,
        AmazonDynamoDB,
        etcd,
        Riak,
        Hazelcast,
        Aerospike,
        ApacheCassandra,
        ApacheHBase,
        ScyllaDB,
        GoogleBigtable,
        Neo4j,
        AmazonNeptune,
        ArangoDB,
        JanusGraph,
        OrientDB,
        TigerGraph,
        InfluxDB,
        TimescaleDB,
        Prometheus,
        OpenTSDB,
        Graphite,
    }
    public class DbCharacteristics
    {
        public bool Polyglot { get; set; }
        public bool Parallel { get; set; }  // Teradata, Greenplum, Netezza
        public bool Hosted { get; set; }    // All major cloud providers' managed services
        public bool Hubrid { get;set; } // MemSQL, SAP HANA, Oracle Database In-Memory
    }
    [Flags]
    public enum DbModelType
    {
        Unknown,
        Relational, // Characteristics: Tabular structure, ACID compliance, SQL querying
        NoSql,
        MultiModel, // ArangoDB, Cosmos DB, OrientDB, MarkLogic
        ObjectOriented, // db4o, ObjectDB, Versant
        Vector,         // Pinecone, Weaviate, Milvus, Qdrant, Chroma
        Xml,             // BaseX, eXist-db, MarkLogic
    }
    [Flags]
    public enum DbUsageType
    {
        Any,
        Quantum,
        HomomorphicEncryption,
        Neuromorphic,
        EventStore,     // EventStoreDB, Apache Kafka (as storage)
        SearchEngine,   // Elasticsearch, Solr, OpenSearch
        Content,        // Jackrabbit, ModeShape
        Mobile,         // Realm, Couchbase Lite, SQLite
        BlockChain,     // BigchainDB, Amazon Quantum Ledger Database
        DatawareHouse,  // Snowflake, Redshift, BigQuery, Databricks
        DataLake,       // Hadoop HDFS, AWS S3, Azure Data Lake
        Spatial,        // PostGIS (PostgreSQL extension), Oracle Spatial
        Temporal,       // Teradata Temporal, Oracle Flashback
        Scientific,     // SciDB, Rasdaman
        AIML,
        SemanticSearch,
        ImageVideoSearch,
        QueryEngine,    // Presto, Apache Drill, Dremio
        Edge,           // SQLite, Raima Database Manager
        Specialized,    /*
                            Genomic: GA4GH, GenBank
                            Chemical: ChemFinder, Cambridge Structural Database
                            Bibliographic: PubMed, IEEE Xplore
                            Financial: Bloomberg Terminal, Reuters databases
                        */
    }
    public class DbSpecification
    {
        public DbVendor Vendor { get; set; }
        public DbProduct Product { get; set; }
        public DataModel DataModel { get; set; }
        public ArchitectureModel ArchitectureModel { get; set; }
        public NoSqlModel NoSqlModel { get; set; }
        public ConsistencyModel ConsistencyModel { get; set; }
        public SchemaChangeApproach SchemaChangeApproach { get; set; }
        public SchemaCheckApproach SchemaCheckApproach { get; set; }
        public DataRelationshipModel DataRelationshipModel { get; set; }
        public PersistenceModel PersistenceModel { get; set; }
        public IndexingModel IndexingModel { get; set; }
        public StorageModel StorageModel { get; set; }
        public QueryLanguageModel QueryLanguageModel { get; set; }
        public QueryCapabilities QueryCapabilities { get; set; }
        public DbCharacteristics DbCharacteristics { get; set; }
        public DbModelType DbModelType { get; set; }
        public DbUsageType DbUsageType { get; set; }
    }
    public interface IDb
    {
        IConnectionStringProvider ConnectionStringProvider { get; set; }
        IDbContextInfoProvider DbContextInfoProvider { get; set; }
        IMapper Mapper { get; set; }
        DbConnection GetConnection();
        bool PersistConnection { get; set; }
        bool AutoNullEmptyStrings { get; set; }
        DbSpecification Specification { get; }
    }
}
