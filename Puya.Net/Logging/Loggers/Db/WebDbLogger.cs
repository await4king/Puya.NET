using Puya.Data;

namespace Puya.Logging
{
    public abstract class WebDbLogger<TConfig> : DbLogger<TConfig>
        where TConfig : WebDbLoggerConfig, new()
    {
        public WebDbLogger(IDb db) : this(null, db, null)
        { }
        public WebDbLogger(TConfig config, IDb db) : this(config, db, null)
        { }
        public WebDbLogger(TConfig config, IDb db, ILogger next) : base(config, db, next)
        {
        }
        protected override bool CanLog(Log log)
        {
            Config.Init();

            var result = base.CanLog(log);

            if (result)
            {
                result = Config.WebPolicy?.CanLog(log) ?? true;
            }

            return result;
        }
        protected override Log Init(Log log)
        {
            var result = base.Init(log);

            if (result != null)
            {
                Config.WebPolicy?.Prepare(result as WebLog);
            }

            return result;
        }
    }
}
