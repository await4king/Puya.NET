using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puya.Caching;
using Puya.Data;
using Puya.Logging;
using Puya.Service;
using Puya.Settings;
using Puya.Translation;

namespace Puya.ServiceModel
{
    public abstract class TapBaseConfig : ServiceConfig
    {
        public ILogger Logger { get; set; }
        public IDb Db { get; set; }
        public ICacheManager Cache { get; set; }
        public ISettingService Settings { get; set; }
        public ITranslator Translator { get; set; }
        public TapBaseConfig()
        { }
        public TapBaseConfig(ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator)
        {
            Logger = logger;
            Db = db;
            Cache = cache;
            Settings = settings;
            Translator = translator;
        }
    }
}
