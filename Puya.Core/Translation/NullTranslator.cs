using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Puya.Localization;

namespace Puya.Translation
{
    public class NullTranslator : ITranslator
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
        public ILanguageProvider LanguageProvider { get; set; }
        public string this[string key]
        {
            get { return ""; }
        }
        public ConcurrentDictionary<string, string[]> GetAll(string storename = "")
        {
            return new ConcurrentDictionary<string, string[]>();
        }
        public string[] Get(string key)
        {
            return new string[0];
        }
        public void Clear()
        {
        }
        public void Load()
        {
        }
    }
}
