using Microsoft.Extensions.Primitives;
using Puya.Collections;
using Puya.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Puya.Logging
{
    public static class Extensions
    {
        public static string Join(this IEnumerable<KeyValuePair<string, StringValues>> collection, StringList include, StringList exclude)
        {
            if (collection != null)
            {
                try
                {
                    return collection.Where(x => (include.Contains("*") || include.Contains(x.Key)) && !(exclude.Contains("*") || exclude.Contains(x.Key))).Join("\n");
                }
                catch
                {
                    return string.Empty;
                }

            }

            return string.Empty;
        }
        public static string Join(this IEnumerable<KeyValuePair<string, string>> collection, StringList include, StringList exclude)
        {
            if (collection != null)
            {
                try
                {
                    return collection.Where(x => (include.Contains("*") || include.Contains(x.Key)) && !(exclude.Contains("*") || exclude.Contains(x.Key))).Join("\n");
                }
                catch
                {
                    return string.Empty;
                }

            }

            return string.Empty;
        }
    }
}
