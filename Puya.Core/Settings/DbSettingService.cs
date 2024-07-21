using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Caching;
using Puya.Conversion;
using Puya.Data;
using Puya.Extensions;
using Puya.Logging;

namespace Puya.Settings
{
    public class DbSettingService: ISettingService
    {
        #region Properties
        private IDb _db;
        public IDb Db
        {
            get
            {
                if (_db == null)
                    _db = new NullDb();

                return _db;
            }
            set { _db = value; }
        }
        private ILogger _logger;
        public ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = new NullLogger();

                return _logger;
            }
            set { _logger = value; }
        }
        private ICache _cache;
        public ICache Cache
        {
            get
            {
                if (_cache == null)
                    _cache = new NullCache();

                return _cache;
            }
            set { _cache = value; }
        }
        private string tableName;
        public virtual string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        private int cacheDuration;
        public int CacheDuration
        {
            get { return cacheDuration; }
            set { cacheDuration = value; }
        }
        public int? AppId { get; set; }
        #endregion
        #region ctor
        public DbSettingService(IDb db): this(db, null, null)
        { }
        public DbSettingService(IDb db, ILogger logger) : this(db, logger, null)
        { }
        public DbSettingService(IDb db, ILogger logger, ICache cache)
        {
            Db = db;
            Logger = logger;
            Cache = cache;

            tableName = "Settings";
            CacheDuration = 10;
        }
        #endregion

        public virtual string this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }
        private string AppCondition()
        {
            return AppId.HasValue ? $" and AppId = {AppId.Value}" : "";
        }
        public int Count(string key = "")
        {
            var keyCondition = string.IsNullOrEmpty(key) ? "" : " and [Key] = @Key";

            var result = Db.ExecuteScalerSql($"select count(*) from {TableName} where 1 = 1 {keyCondition} {AppCondition()}", new
            {
                Key = key
            });

            return SafeClrConvert.ToInt(result);
        }
        string GetCacheKey(string key)
        {
            return $"{(AppId.HasValue ? AppId.Value + ".": "")}{key}";
        }
        public string Get(string key)
        {
            try
            {
                var cacheKey = GetCacheKey(key);
                var result = Cache.GetOrSet(cacheKey, () => Db.ExecuteScalerSql($"select top 1 [Value] from {TableName} where [Key] = @Key {AppCondition()}", new { Key = key }), CacheDuration);
                
                return result?.ToString();
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return "";
        }
        public async Task<string> GetAsync(string key, CancellationToken cancellation)
        {
            try
            {
                object result = null;

                var cacheKey = GetCacheKey(key);

                if (Cache.Exists(cacheKey))
                {
                    result = Cache.Get<string>(cacheKey);
                }
                else
                {
                    result = await Db.ExecuteScalerSqlAsync($"select top 1 [Value] from {TableName} where [Key] = @Key {AppCondition()}", new { Key = key }, cancellation);

                    Cache.Set(cacheKey, result, CacheDuration);
                }

                return result?.ToString();
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return "";
        }
        public bool Set(string key, string value)
        {
            if (Count(key) > 1)
            {
                throw new Exception($"More than one setting found for '{key}'. Update will modify all of them and losing data.");
            }

            try
            {
                var query = $@"
                                update {TableName} set [Value] = @Value where [Key] = @Key {AppCondition()};
                                if @@rowcount = 0
                                    insert into {TableName}([AppId], [Key], [Value]) values (@AppId, @Key, @Value)
                            ";
                var rows = Db.ExecuteNonQuerySql(query, new { AppId, Key = key, Value = value });
                var result = rows == 1;

                if (result)
                {
                    var cacheKey = GetCacheKey(key);

                    Cache.Remove(cacheKey);
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return false;
        }
        public async Task<bool> SetAsync(string key, string value, CancellationToken cancellation)
        {
            try
            {
                var query = $@"
                                update {TableName} set [Value] = @Value where [Key] = @Key {AppCondition()};
                                if @@rowcount = 0
                                    insert into {TableName}([AppId], [Key], [Value]) values (@AppId, @Key, @Value)
                            ";
                var rows = await Db.ExecuteNonQuerySqlAsync(query, new { AppId, Key = key, Value = value }, cancellation);
                var result = rows == 1;

                if (result)
                {
                    var cacheKey = GetCacheKey(key);

                    Cache.Remove(cacheKey);
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return false;
        }
        public IDictionary<string, string> GetAll()
        {
            try
            {
                var result = Db.ExecuteReaderSql($"select [Key], [Value] from {TableName} where 1 = 1 {AppCondition()}",
                                    reader => new KeyValuePair<string, string>(SafeClrConvert.ToString(reader[0]), SafeClrConvert.ToString(reader[1])));

                return result.ToDictionary();
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return new Dictionary<string, string>();
        }
        public async Task<IDictionary<string, string>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                var result = await Db.ExecuteReaderSqlAsync($"select [Key], [Value] from {TableName} where 1 = 1 {AppCondition()}",
                                    reader => new KeyValuePair<string, string>(SafeClrConvert.ToString(reader[0]), SafeClrConvert.ToString(reader[1])),
                                    cancellation);

                return result.ToDictionary();
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return new Dictionary<string, string>();
        }

        public IDictionary<string, string> GetRange(string prefix)
        {
            try
            {
                var result = Db.ExecuteReaderSql($"select [Key], [Value] from {TableName} where [Key] like @prefix + '%' {AppCondition()}",
                                    reader => new KeyValuePair<string, string>(SafeClrConvert.ToString(reader[0]), SafeClrConvert.ToString(reader[1]))
                                    , new { prefix });

                return result.ToDictionary();
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return new Dictionary<string, string>();
        }

        public async Task<IDictionary<string, string>> GetRangeAsync(string prefix, CancellationToken cancellation)
        {
            try
            {
                var result = await Db.ExecuteReaderSqlAsync($"select [Key], [Value] from {TableName} where [Key] like @prefix + '%' {AppCondition()}",
                                    reader => new KeyValuePair<string, string>(SafeClrConvert.ToString(reader[0]), SafeClrConvert.ToString(reader[1]))
                                    , new { prefix }, cancellation);

                return result.ToDictionary();
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }

            return new Dictionary<string, string>();
        }
    }
}
