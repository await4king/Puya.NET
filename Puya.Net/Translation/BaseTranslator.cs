using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puya.Caching;
using Puya.Collections;
using Puya.Extensions;
using Puya.Localization;
using Puya.Logging;

namespace Puya.Translation
{
    public abstract class BaseTranslator : ITranslator
    {
        private ITranslatorOptions options;
        public ITranslatorOptions Options
        {
            get
            {
                if (options == null)
                {
                    options = new TranslatorOptions();
                }

                return options;
            }
            set { options = value; }
        }
        protected ConcurrentDictionary<string, string[]> _store;
        protected ConcurrentDictionary<string, string[]> _storenames;
        private ConcurrentDictionary<string, string[]> Store
        {
            get
            {
                if (_store == null || (_store?.Count == 0 && !loaded))
                {
                    Load();
                }

                return _store;
            }
        }
        protected bool loaded;
        private string cacheName;
        public virtual string CacheName
        {
            get
            {
                if (string.IsNullOrEmpty(cacheName))
                {
                    cacheName = this.GetType().FullName;
                }

                return cacheName;
            }
            set
            {
                Clear();

                cacheName = value;

                Load();
            }
        }
        private string CacheCategoryName
        {
            get
            {
                return CacheName + ".names";
            }
        }
        public virtual string Name
        {
            get
            {
                return this.GetType().FullName;
            }
        }
        protected ICache cache;
        public ICache Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new NullCache();
                }

                return cache;
            }
            set
            {
                cache = value;
            }
        }
        private ILogger logger;
        public ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = new NullLogger();
                }

                return logger;
            }
            set
            {
                logger = value;
            }
        }
        private ILanguageProvider languageProvider;
        public ILanguageProvider LanguageProvider
        {
            get
            {
                if (languageProvider == null)
                {
                    languageProvider = new FixedLanguageProvider("en");
                }

                return languageProvider;
            }
            set
            {
                languageProvider = value;
            }
        }
        protected virtual void Init(ICache cache, ILogger logger, ILanguageProvider languageProvider) { }
        private void InitInternal(ICache cache, ILogger logger, ILanguageProvider languageProvider)
        {
            if (cache == null)
            {
                var appCache = new AppDomainCache();

                appCache.Duration = 3600;

                this.cache = appCache;
            }
            else
            {
                this.cache = cache;
            }

            LanguageProvider = languageProvider;
            Logger = logger;

            Logger.Info($"{Name}.init()");
            Logger.Debug($"Cache: {Cache.GetType().Name}");
            Logger.Debug($"Logger: {Logger.GetType().Name}");

            try
            {
                Logger.Debug($"Restoring items from cache");

                _store = (ConcurrentDictionary<string, string[]>)Cache.Get(CacheName);
                _storenames = (ConcurrentDictionary<string, string[]>)Cache.Get(CacheCategoryName);
            }
            catch (Exception e)
            {
                Logger.Debug($"Init failed.");

                Logger.Danger(e);

                _store = null;
                _storenames = null;
            }

            if (_store == null)
            {
                _store = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                _storenames = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
            }

            Init(cache, logger, languageProvider);
        }
        public BaseTranslator() : this(null, null, null)
        { }
        public BaseTranslator(ICache cache, ILogger logger) : this(cache, logger, null)
        { }
        public BaseTranslator(ICache cache, ILogger logger, ILanguageProvider languageProvider)
        {
            InitInternal(cache, logger, languageProvider);
        }
        public void Clear()
        {
            _store?.Clear();
            loaded = false;
        }
        public virtual ConcurrentDictionary<string, string[]> GetAll(string category = "")
        {
            if (string.IsNullOrEmpty(category))
            {
                return Store;
            }
            else
            {
                var result = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                var _storename_keys = _storenames?.FirstOrDefault(name => string.Compare(name.Key, category, true) == 0).Value;

                if (_storename_keys?.Length > 0)
                {
                    foreach (var item in Store)
                    {
                        if (_storename_keys.FindIndexOf(item.Key, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            result.TryAdd(item.Key, item.Value);
                        }
                    }
                }

                return result;
            }
        }
        protected abstract void LoadInternal();
        public virtual void Load()
        {
            void UpdateLinkedTexts()
            {
                if (_store == null)
                {
                    _store = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                    _storenames = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                }

                foreach (var item in _store)
                {
                    var values = item.Value;
                    if (values != null && values.Length > 0)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            var value = values[i]?.Trim();

                            values[i] = CheckValue(value);
                        }
                    }
                }
            }

            Logger.Debug($"{Name}.Load()");

            if (!loaded)
            {
                Clear();

                try
                {
                    LoadInternal();

                    Logger.Debug($"Updating linked texts ...");

                    UpdateLinkedTexts();

                    Logger.Debug($"Caching ...");

                    Cache.GetOrSet(CacheName, _store);
                    Cache.GetOrSet(CacheCategoryName, _storenames);

                    Logger.Debug($"succeeded.");

                    loaded = true;
                }
                catch (Exception e)
                {
                    Logger.Debug($"load error");

                    Logger.Danger(e);

                    if (_store == null)
                    {
                        _store = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                        _storenames = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                    }
                }
            }
            else
            {
                Logger.Debug($"Already loaded");
            }
        }
        internal static string[] GetValues(string values)
        {
            var result = new List<string>();
            var _values = values?.Trim();

            if (!string.IsNullOrEmpty(_values))
            {
                var buffer = new CharBuffer(32);
                var i = 0;
                var state = 0;

                while (i < _values.Length)
                {
                    var ch = _values[i++];

                    switch (state)
                    {
                        case 0:
                            switch (ch)
                            {
                                case '\\':
                                    state = 1;
                                    break;
                                case ',':
                                    var value = buffer.ToString();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        result.Add(value);
                                    }
                                    buffer.Reset();
                                    break;
                                default:
                                    buffer.Append(ch);
                                    break;
                            }

                            break;
                        case 1:
                            switch (ch)
                            {
                                case ',':
                                    buffer.Append(',');
                                    break;
                                case 'n':
                                    buffer.Append('\n');
                                    break;
                                case 'r':
                                    buffer.Append('\r');
                                    break;
                                case 'f':
                                    buffer.Append('\f');
                                    break;
                                case 'b':
                                    buffer.Append('\b');
                                    break;
                                case 'v':
                                    buffer.Append('\v');
                                    break;
                                case '\\':
                                    buffer.Append('\\');
                                    break;
                                default:
                                    buffer.Append('\\');
                                    buffer.Append(ch);
                                    break;
                            }

                            state = 0;

                            break;
                    }
                }

                if (buffer.Length > 0)
                {
                    var value = buffer.ToString();

                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();
        }
        public string[] Get(string key)
        {
            var result = new string[] { };

            if (!string.IsNullOrEmpty(key))
            {
                if (Options.MapKeyOptions != null)
                {
                    key = (Options.MapKeyOptions.RequiresFirstSeparator ? Options.MapKeyOptions.PartSeparator : "")
                            +
                          key.Replace(Options.MapKeyOptions.PartSeparator, Options.KeyOptions.PartSeparator);
                }

                Store.TryGetValue(key, out result);
            }

            return result;
        }
        protected virtual void ProcessCultureDependentText(string category, string content)
        {
            var lines = content?.Split('\n', MyStringSplitOptions.TrimAndRemoveEmptyEntries) ?? new string[] { };
            var keys = new List<string>();
            var itemCount = 0;
            var skippedItemsCount = 0;
            var commentCount = 0;

            Logger.Debug($"total lines: {lines.Length}");

            foreach (var l in lines)
            {
                var line = l.Trim();

                if (line.Length > 0)
                {
                    if (Array.IndexOf(Options.CommentCharacters, line[0]) < 0)
                    {
                        var colonIndex = line.IndexOf(Options.KeyValueSeparator);

                        if (colonIndex > 1 && colonIndex < line.Length)
                        {
                            var left = line.Substring(0, colonIndex).Trim();
                            var right = line.Substring(colonIndex + 1).Trim();
                            var path = left.Split(Options.KeyOptions.PartSeparator, MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries);
                            var values = GetValues(right);

                            if (path.Length == (3 + (Options.KeyOptions.RequiresFirstSeparator ? 1 : 0)))
                            {
                                var key = Options.KeyOptions.PartSeparator + String.Join(Options.KeyOptions.PartSeparator.ToString(), path);

                                if (_store == null)
                                {
                                    _store = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                                }

                                if (_store.TryAdd(key, values))
                                {
                                    itemCount++;

                                    if (!string.IsNullOrEmpty(category))
                                    {
                                        keys.Add(key);
                                    }
                                }
                                else
                                {
                                    skippedItemsCount++;

                                    Logger.Debug($"Key already exists: {key}");
                                }
                            }
                        }
                    }
                    else
                    {
                        commentCount++;
                    }
                }
            }

            Logger.Debug($"comment lines: {commentCount}, item lines: {itemCount}, skipped items: {skippedItemsCount}");

            if (!string.IsNullOrEmpty(category))
            {
                if (_storenames == null)
                {
                    _storenames = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                }

                _storenames.TryAdd(category, keys.ToArray());
            }
        }
        protected virtual void ProcessCultureIndependentText(string category, string content)
        {
            var lines = content?.Split('\n', MyStringSplitOptions.TrimAndRemoveEmptyEntries) ?? new string[] { };
            var keys = new List<string>();
            var itemCount = 0;
            var skippedItemsCount = 0;
            var commentCount = 0;

            Logger.Debug($"total lines: {lines.Length}");

            foreach (var l in lines)
            {
                var line = l.Trim();

                if (line.Length > 0)
                {
                    if (Array.IndexOf(Options.CommentCharacters, line[0]) < 0)
                    {
                        var colonIndex = line.IndexOf(Options.KeyValueSeparator);

                        if (colonIndex > 1 && colonIndex < line.Length)
                        {
                            var left = line.Substring(0, colonIndex).Trim();
                            var right = line.Substring(colonIndex + 1).Trim();
                            var path = left.Split(Options.KeyOptions.PartSeparator, MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries);
                            var values = GetValues(right);

                            if (path.Length == (2 + (Options.KeyOptions.RequiresFirstSeparator ? 1 : 0)))
                            {
                                var key = Options.KeyOptions.PartSeparator + String.Join(Options.KeyOptions.PartSeparator, path);

                                if (_store == null)
                                {
                                    _store = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                                }

                                if (_store.TryAdd(key, values))
                                {
                                    itemCount++;

                                    if (!string.IsNullOrEmpty(category))
                                    {
                                        keys.Add(key);
                                    }
                                }
                                else
                                {
                                    skippedItemsCount++;

                                    Logger.Debug($"Key already exists: {key}");
                                }
                            }
                        }
                    }
                    else
                    {
                        commentCount++;
                    }
                }
            }

            Logger.Debug($"comment lines: {commentCount}, item lines: {itemCount}, skipped items: {skippedItemsCount}");

            if (!string.IsNullOrEmpty(category))
            {
                if (_storenames == null)
                {
                    _storenames = new ConcurrentDictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                }

                if (!_storenames.TryAdd(category, keys.ToArray()))
                {
                    Logger.Debug($"storename already exists: {category}");
                }
            }
        }
        private string CheckValue(string value)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value) && value.Length > 2)
            {
                var temp = "";
                var sb = new StringBuilder();
                var link = "";
                var state = 0;

                foreach (var ch in value)
                {
                    switch (state)
                    {
                        case 0:
                            if (ch == '\\')
                            {
                                if (!string.IsNullOrEmpty(temp))
                                {
                                    sb.Append(temp);
                                }
                                temp = "";
                                state = 1;
                            }
                            else
                            if (ch == '{')
                            {
                                if (!string.IsNullOrEmpty(temp))
                                {
                                    sb.Append(temp);
                                }
                                temp = "";
                                state = 2;
                            }
                            else
                            {
                                temp += ch;
                            }

                            break;
                        case 1:
                            if (ch == '{')
                            {
                                sb.Append('{');
                            }
                            else
                            {
                                sb.Append('\\' + ch);
                            }
                            state = 0;
                            break;
                        case 2:
                            if (ch == '}')
                            {
                                temp = this.GetSingle(link);

                                if (!string.IsNullOrEmpty(temp))
                                {
                                    sb.Append(temp);
                                }
                                else
                                {
                                    sb.Append("{" + link + "}");
                                }

                                link = "";
                                temp = "";
                                state = 0;
                            }
                            else
                            {
                                link += ch;
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(temp))
                {
                    sb.Append(temp);
                }

                result = sb.ToString();
            }

            return result;
        }
        public string this[string key]
        {
            get
            {
                return this.GetSingle(key);
            }
        }
    }
}
