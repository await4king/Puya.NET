using System.Linq;

namespace Puya.Base
{
    public static class LanguageConstructs
    {
        public static bool IsSomeString(string s, bool rejectAllWhitespaceStrings = false)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return !rejectAllWhitespaceStrings || s.ToCharArray().Any(ch => !char.IsWhiteSpace(ch));
            }

            return false;
        }
    }
}
