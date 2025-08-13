using Puya.Collections;

namespace Puya.Logging
{
    public class LoggingPolicyOptions
    {
        public bool Form { get; set; }
        public StringList FormIncludeKeys { get; set; }
        public StringList FormExcludeKeys { get; set; }
        public bool Headers { get; set; }
        public StringList HeadersIncludeKeys { get; set; }
        public StringList HeadersExcludeKeys { get; set; }
        public bool Cookies { get; set; }
        public StringList CookiesIncludeKeys { get; set; }
        public StringList CookiesExcludeKeys { get; set; }
        public bool Body { get; set; }
        public bool Persist { get; set; }
        public LoggingPolicyOptions()
        {
            FormIncludeKeys = new StringList();
            FormExcludeKeys = new StringList();

            HeadersIncludeKeys = new StringList();
            HeadersExcludeKeys = new StringList();

            CookiesIncludeKeys = new StringList();
            CookiesExcludeKeys = new StringList();
        }
    }
}