using Puya.Collections;
using Puya.Data;
using System.Collections.Generic;
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
        protected abstract string GetFetchLogsQuery(out CommandType commandType);
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
        protected override void ClearInternal()
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
        protected override async Task ClearInternalAsync(CancellationToken cancellation)
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
        public IList<Log> FetchLogs(object args = null)
        {
            IList<Log> result = new List<Log>();

            if (Db != null)
            {
                var query = GetFetchLogsQuery(out CommandType commandType);

                if (string.IsNullOrEmpty(query))
                {
                    throw new System.Exception("fetch logs query not specified");
                }

                if (commandType == CommandType.Text)
                {
                    result = Db.ExecuteReaderSql<Log>(query, args);
                }
                else if (commandType == CommandType.StoredProcedure)
                {
                    result = Db.ExecuteReaderCommand<Log>(query, args);
                }

                if (result?.Count > 0)
                {
                    foreach (var log in result)
                    {
                        var data = log.Data?.ToString();

                        log.Data = Config.Formatter.DeserializeData(data);
                    }
                }
            }

            return result;
        }
    }
}
