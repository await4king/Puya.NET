using Puya.Collections;
using Puya.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public abstract class DbLogger<TConfig> : BaseLogger<TConfig>
        where TConfig: DbLoggerConfig, new()
    {
        public virtual IDb Db { get; set; }
        #region ctor
        public DbLogger(IDb db) : this(null, db, null)
        { }
        public DbLogger(TConfig config, IDb db) : this(config, db, null)
        { }
        public DbLogger(TConfig config, IDb db, ILogger next) : base(config, next)
        {
            Db = db;
        }
        #endregion
        protected abstract string GetInsertLogQuery(out CommandType commandType);
        protected abstract DynamicModel GetInsertLogArgs(Log log);
        protected abstract DynamicModel GetClearLogArgs();
        protected override void LogInternal(Log log)
        {
            if (Db != null)
            {
                var query = GetInsertLogQuery(out CommandType commandType);

                if (string.IsNullOrEmpty(query))
                {
                    throw new System.Exception("log insertion query not specified");
                }

                var args = GetInsertLogArgs(log);

                if (commandType == CommandType.Text)
                {
                    Db.ExecuteNonQuerySql(query, args);
                }
                else if (commandType == CommandType.StoredProcedure)
                {
                    Db.ExecuteNonQueryCommand(query, args);
                }
            }
        }
        protected override async Task LogInternalAsync(Log log, CancellationToken cancellation)
        {
            if (Db != null)
            {
                var query = GetInsertLogQuery(out CommandType commandType);

                if (string.IsNullOrEmpty(query))
                {
                    throw new System.Exception("log insertion query not specified");
                }

                var args = GetInsertLogArgs(log);

                if (commandType == CommandType.Text)
                {
                    await Db.ExecuteNonQuerySqlAsync(query, args, cancellation);
                }
                else if (commandType == CommandType.StoredProcedure)
                {
                    await Db.ExecuteNonQueryCommandAsync(query, args, cancellation);
                }
            }
        }
        protected abstract string GetClearQuery(out CommandType commandType);
        public override void Clear()
        {
            if (Db != null)
            {
                var query = GetClearQuery(out CommandType commandType);

                if (string.IsNullOrEmpty(query))
                {
                    throw new System.Exception("log clear query not specified");
                }

                var args = GetClearLogArgs();

                if (commandType == CommandType.Text)
                {
                    Db.ExecuteNonQuerySql(query, args);
                }
                else if (commandType == CommandType.StoredProcedure)
                {
                    Db.ExecuteNonQueryCommand(query, args);
                }
            }
        }
        public override async Task ClearAsync(CancellationToken cancellation)
        {
            if (Db != null)
            {
                var query = GetClearQuery(out CommandType commandType);

                if (string.IsNullOrEmpty(query))
                {
                    throw new System.Exception("log clear query not specified");
                }

                var args = GetClearLogArgs();

                if (commandType == CommandType.Text)
                {
                    await Db.ExecuteNonQuerySqlAsync(query, args, cancellation);
                }
                else if (commandType == CommandType.StoredProcedure)
                {
                    await Db.ExecuteNonQueryCommandAsync(query, args, cancellation);
                }
            }
        }
    }
}
