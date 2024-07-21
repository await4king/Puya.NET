using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Puya.Caching;
using Puya.Conversion;
using Puya.Data;
using Puya.Extensions;
using Puya.Localization;
using Puya.Logging;

namespace Puya.Translation
{
    public class DbTranslator : BaseTranslator
    {
        private string tableName;
        public virtual string TableName
        {
            get { return tableName; }
            set { tableName = value?.Replace("'", ""); }
        }
        public int? AppId { get; set; }
        private IDb db;
        public IDb Db
        {
            get
            {
                if (db == null)
                    db = new NullDb();

                return db;
            }
            set { db = value; }
        }
        public DbTranslator(IDb db) : this(db, null, null)
        { }
        public DbTranslator(IDb db, ILogger logger) : this(db, null, logger)
        { }
        public DbTranslator(IDb db, ICache cache) : this(db, cache, null)
        { }
        public DbTranslator(IDb db, ICache cache, ILogger logger) : this(db, cache, logger, null)
        { }
        public DbTranslator(IDb db, ICache cache, ILogger logger, ILanguageProvider languageProvider) : base(cache, logger, languageProvider)
        {
            this.db = db;
            tableName = "Texts";
        }
        private string AppCondition()
        {
            return AppId.HasValue ? $" and AppId = {AppId.Value}" : "";
        }
        private bool IsCultureDependent(string key)
        {
            return string.IsNullOrEmpty(key) ? false : key.Length - key.Replace(Options.KeyOptions.PartSeparator, "").Length == 3 + (Options.KeyOptions.RequiresFirstSeparator ? 1 : 0);
        }
        private bool IsCultureIndependent(string key)
        {
            return string.IsNullOrEmpty(key) ? false: key.Length - key.Replace(Options.KeyOptions.PartSeparator, "").Length == 2 + (Options.KeyOptions.RequiresFirstSeparator ? 1 : 0);
        }
        protected override void LoadInternal()
        {
            try
            {
                var appCondition = AppCondition();
                var all = Db.ExecuteReaderSql($@"select [Category], [Key], [Value] from {TableName} where 1 = 1 {appCondition}",
                    reader => new Tuple<string, string, string>(SafeClrConvert.ToString(reader[0]), SafeClrConvert.ToString(reader[1]), SafeClrConvert.ToString(reader[2])));
                var categories = all.Where(x => IsCultureDependent(x.Item2))?.Select(x => x.Item1)?.Distinct()?.ToList() ?? new List<string>();

                foreach (var category in categories)
                {
                    var content = all.Where(x => x.Item1 == category && IsCultureDependent(x.Item2))?.Select(x => x.Item2 + Options.KeyValueSeparator + x.Item3)?.Join("\n");

                    if (!string.IsNullOrEmpty(content))
                    {
                        ProcessCultureDependentText(category, content);
                    }
                }

                categories = all.Where(x => IsCultureIndependent(x.Item2))?.Select(x => x.Item1)?.Distinct()?.ToList() ?? new List<string>();

                foreach (var category in categories)
                {
                    var content = all.Where(x => x.Item1 == category && IsCultureIndependent(x.Item2))?.Select(x => x.Item2 + Options.KeyValueSeparator + x.Item3)?.Join("\n");

                    if (!string.IsNullOrEmpty(content))
                    {
                        ProcessCultureIndependentText(category, content);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Danger(e);
            }
        }
    }
}
