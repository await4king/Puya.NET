using System;
using System.Linq;
using Puya.Extensions;

namespace Puya.Service
{
    public class LogProviderOptions
    {
        string include;
        string[] includes;
        public string Include
        {
            get
            {
                return include;
            }
            set
            {
                includes = value?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? null;

                if (includes?.Length == 0)
                {
                    includes = new string[] { "*" };
                }

                include = includes.Join(",");
            }
        }
        string exclude;
        string[] excludes;
        public string Exclude
        {
            get
            {
                return exclude;
            }
            set
            {
                excludes = value?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
                exclude = excludes.Join(",");
            }
        }
        public bool Includes(string level)
        {
            return string.IsNullOrWhiteSpace(Include) || (includes?.Contains("*", StringComparer.OrdinalIgnoreCase) ?? false) || (includes?.Contains(level, StringComparer.OrdinalIgnoreCase) ?? false);
        }
        public bool Excludes(string level)
        {
            return excludes?.Contains(level, StringComparer.OrdinalIgnoreCase) ?? false;
        }
    }
}
