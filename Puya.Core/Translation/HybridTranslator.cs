using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Puya.Caching;
using Puya.Data;
using Puya.Extensions;
using Puya.Localization;
using Puya.Logging;

namespace Puya.Translation
{
    public class HybridTranslator : ITranslator
    {
        private ITranslatorOptions options;
        public ITranslatorOptions Options
        {
            get
            {
                if (options == null)
                {
                    options = Translators.Count > 0 ? Translators[0].Options: new TranslatorOptions();
                }

                return options;
            }
            set
            {
                options = value;

                for (var i = 0; i < Translators.Count; i++)
                {
                    var translator = Translators[i];

                    translator.Options = options;
                }
            }
        }
        public ILanguageProvider LanguageProvider { get; set; }
        private FileBasedTranslator textTranslator;
        private ResourceBasedTranslator resourceTranslator;
        private DbTranslator dbTranslator;
        private List<ITranslator> translators;
        public List<ITranslator> Translators
        {
            get { return translators; }
        }
        public FileBasedTranslator File
        {
            get { return textTranslator; }
        }
        public ResourceBasedTranslator Resource
        {
            get { return resourceTranslator; }
        }
        public DbTranslator Db
        {
            get { return dbTranslator; }
        }
        public string this[string key]
        {
            get
            {
                return this.GetSingle(key);
            }
        }

        public HybridTranslator(): this(null, null, null, null)
        { }
        public HybridTranslator(ICache cache, ILogger logger, IDb db, ILanguageProvider languageProvider)
        {
            textTranslator = new FileBasedTranslator(cache, logger, languageProvider);
            resourceTranslator = new ResourceBasedTranslator(cache, logger, languageProvider);
            dbTranslator = new DbTranslator(db, cache, logger, languageProvider);
            
            translators = new List<ITranslator>();

            translators.Add(textTranslator);
            translators.Add(resourceTranslator);
            translators.Add(dbTranslator);

            LanguageProvider = languageProvider;
        }
        public void Clear()
        {
            foreach (var translator in translators)
            {
                translator.Clear();
            }
        }
        public string[] Get(string key)
        {
            var result = new string[] { };

            foreach (var translator in translators)
            {
                result = translator.Get(key);

                if (result != null && result.Length > 0)
                {
                    break;
                }
            }

            return result;
        }
        public ConcurrentDictionary<string, string[]> GetAll(string storename = "")
        {
            var result = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);

            foreach (var translator in translators)
            {
                var r = translator.GetAll(storename);

                result.Merge(r);
            }

            return result;
        }
        public void Load()
        {
            foreach (var translator in translators)
            {
                translator.Load();
            }
        }
    }
}
