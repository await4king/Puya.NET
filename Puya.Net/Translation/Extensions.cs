using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Translation
{
    public static class Extensions
    {
        private static string GetKey(ITranslator translator, string key, object globalValue, string culture, string lang)
        {
            var result = "";

            lang = string.IsNullOrEmpty(lang) ? translator?.LanguageProvider?.GetCurrent() : lang;

            if (key?.Length > 1 && !key.StartsWith(translator.Options.KeyOptions.PartSeparator))
            {
                result = translator.Options.KeyOptions.PartSeparator + key;
            }
            else
            {
                result = key;
            }

            result += translator.Options.KeyOptions.PartSeparator +
                    globalValue +
                    (culture != null ?
                        translator.Options.KeyOptions.PartSeparator +
                        culture : "") +
                    translator.Options.KeyOptions.PartSeparator +
                    lang;

            return result;
        }
        public static string[] Get(this ITranslator translator, string key, object globalValue, string culture, string lang = "")
        {
            var _key = GetKey(translator, key, globalValue, culture, lang);
            
            return translator.Get(_key);
        }
        public static string[] Get(this ITranslator translator, string key, string value, string lang = "")
        {
            var _key = GetKey(translator, key, value, null, lang);
            
            return translator.Get(_key);
        }
        public static string GetSingle(this ITranslator translator, string key, object globalValue, string culture, string lang = "")
        {
            var _key = GetKey(translator, key, globalValue, culture, lang);

            return translator.GetSingle(_key);
        }
        public static string GetSingle(this ITranslator translator, string key, string value, string lang = "")
        {
            var _key = GetKey(translator, key, value, null, lang);
            
            return translator.GetSingle(_key);
        }
        public static string GetSingle(this ITranslator translator, string key)
        {
            var result = null as string;
            var values = translator.Get(key);

            if (values != null && values.Length > 0)
                result = values[0];

            return result;
        }


        public static string GetNthNumber(this ITranslator translator, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_NthNum, n.ToString(), lang);
        }
        public static string GetNumber(this ITranslator translator, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_Number, n.ToString(), lang);
        }
        public static string GetMonthName(this ITranslator translator, string culture, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_Month, n.ToString(), culture, lang);
        }
        public static string GetShortMonthName(this ITranslator translator, string culture, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_ShortMonth, n.ToString(), culture, lang);
        }
        public static string GetWeekdayName(this ITranslator translator, string culture, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_Weekday, n.ToString(), culture, lang);
        }
        public static string GetShortWeekdayName(this ITranslator translator, string culture, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_ShortWeekday, n.ToString(), culture, lang);
        }
        public static string GetSeasonName(this ITranslator translator, string culture, int n, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_Season, n.ToString(), culture, lang);
        }
        public static string GetMeasurementUnit(this ITranslator translator, string unit, string lang = "")
        {
            return translator.GetSingle(TranslationConstants.TK_MeasurementUnit, unit, lang);
        }
    }
}
