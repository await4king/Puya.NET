using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Localization
{
    public enum TextAlign
    {
        Unspecified, left, right, center, justified
    }
    public enum TextDirection
    {
        Unspecified, rtl, ltr
    }
    public enum LangType
    {
        Unknown, fa, en
    }
    public abstract class Language
    {
        public LangType Type { get; protected set; }
        public char DigitSeparator { get; protected set; }
        public string Name { get; protected set; }
        public string Culture { get; protected set; }
        public string LocalName { get; protected set; }
        public string AltName { get; protected set; }
        public string ShortName { get; protected set; }
        public TextDirection Direction { get; protected set; }
        public TextDirection AltDirection { get; protected set; }
        public TextAlign Align { get; protected set; }
        public TextAlign AltAlign { get; protected set; }
        public char[] Digits { get; protected set; }
        public abstract string Render(object text);
        private static readonly LangFa _fa;
        public static LangFa Fa
        {
            get
            {
                return _fa;
            }
        }

        private static readonly LangEn _en;
        public static LangEn En
        {
            get
            {
                return _en;
            }
        }
        public static Language Get(string shortname)
        {
            Language result = null;

            if (string.Compare(shortname, _fa.ShortName, StringComparison.CurrentCultureIgnoreCase) == 0)
                result = _fa;
            else
            if (string.Compare(shortname, _en.ShortName, StringComparison.CurrentCultureIgnoreCase) == 0)
                result = _en;
            else
            if (string.Compare(shortname, _fa.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                result = _fa;
            else
            if (string.Compare(shortname, _en.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                result = _en;

            return result;
        }
        public static List<Language> GetMany(params string[] shortnames)
        {
            var result = new List<Language>();

            foreach (var la in shortnames)
            {
                var lang = Language.Get(la);

                if (lang != null)
                {
                    result.Add(lang);
                }
            }

            return result;
        }
        public static Language Get(LangType type)
        {
            Language result;

            if (type == LangType.fa)
                result = _fa;
            if (type == LangType.en)
                result = _en;

            throw new ApplicationException("language not supported");
        }
        private static readonly List<Language> _langs;
        static Language()
        {
            _fa = new LangFa();
            _en = new LangEn();

            _langs = new List<Language>();

            _langs.Add(_fa);
            _langs.Add(_en);

            Default = _en;
        }
        public static List<Language> GetAll()
        {
            return _langs;
        }
        public static Language Default { get; set; }
    }
}
