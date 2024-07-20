using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Puya.Localization;

namespace Puya.Translation
{
    public interface ITranslator
    {
        ILanguageProvider LanguageProvider { get; set; }
        ITranslatorOptions Options { get; set; }
        ConcurrentDictionary<string, string[]> GetAll(string category = "");
        string[] Get(string key);
        void Clear();
        void Load();
        string this[string key] { get; }
    }
}
