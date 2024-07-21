namespace Puya.Serialization
{
    public class QuerySerializationOptions
    {
        public bool EnumAsString { get; set; }
        public bool ExtendArrays { get; set; }
        public string ArraySeparator { get; set; }
        public bool IgnoreNullOrEmpty { get; set; }
        public bool EncodePropNames { get; set; }
        public bool CaseSensitivePropNames { get; set; }
        public bool UseQuestionMark { get; set; }
        public string DateTimeFormat { get; set; }
        public QuerySerializationOptions()
        {
            EnumAsString = false;
            ExtendArrays = false;
            ArraySeparator = ",";
            IgnoreNullOrEmpty = true;
            EncodePropNames = false;
            CaseSensitivePropNames = false;
            UseQuestionMark = true;
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffZ";
        }
    }
    public static class Querystring
    {
        public static QuerySerializationOptions Options()
        {
            return new QuerySerializationOptions();
        }
        public static QuerySerializationOptions EnumAsString(this QuerySerializationOptions options, bool enumAsString = true)
        {
            options.EnumAsString = enumAsString;

            return options;
        }
        public static QuerySerializationOptions ExtendArrays(this QuerySerializationOptions options, bool extendArrays = true)
        {
            options.ExtendArrays = extendArrays;

            return options;
        }
        public static QuerySerializationOptions IgnoreNullOrEmpty(this QuerySerializationOptions options, bool ignoreNullOrEmpty = true)
        {
            options.IgnoreNullOrEmpty = ignoreNullOrEmpty;

            return options;
        }
        public static QuerySerializationOptions EncodePropNames(this QuerySerializationOptions options, bool encodePropNames = true)
        {
            options.EncodePropNames = encodePropNames;

            return options;
        }
        public static QuerySerializationOptions CaseSensitivePropNames(this QuerySerializationOptions options, bool caseSensitivePropNames = true)
        {
            options.CaseSensitivePropNames = caseSensitivePropNames;

            return options;
        }
        public static QuerySerializationOptions UseQuestionMark(this QuerySerializationOptions options, bool useQuestionMark = true)
        {
            options.UseQuestionMark = useQuestionMark;

            return options;
        }
        public static QuerySerializationOptions ArraySeparator(this QuerySerializationOptions options, string arraySeparator)
        {
            options.ArraySeparator = arraySeparator;

            return options;
        }
        public static QuerySerializationOptions DateTimeFormat(this QuerySerializationOptions options, string dateTimeFormat)
        {
            options.DateTimeFormat = dateTimeFormat;

            return options;
        }
    }
}
