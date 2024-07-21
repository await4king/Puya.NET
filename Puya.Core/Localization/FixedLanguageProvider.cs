using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Localization
{
    public class FixedLanguageProvider : ILanguageProvider
    {
        public string Lang { get; set; }
        public FixedLanguageProvider(string lang)
        {
            Lang = lang;
        }
        public string[] GetAll()
        {
            return new string[] { Lang };
        }
        public string GetCurrent()
        {
            return Lang;
        }
        public string[] GetSupported()
        {
            return new string[] { Lang };
        }
    }
}
