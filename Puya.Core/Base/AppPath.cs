using Puya.Extensions;
using System;

namespace Puya.Base
{
    public class AppPath
    {
        public static string Root
        {
            get
            {
                var result = AppDomain.CurrentDomain.BaseDirectory;

                if (result.EndzWith("\\debug"))
                {
                    result = result.Substring(0, result.Length - 6);
                }
                if (result.EndzWith("\\release"))
                {
                    result = result.Substring(0, result.Length - 8);
                }
                if (result.EndzWith("\\bin"))
                {
                    result = result.Substring(0, result.Length - 4);
                }

                return result;
            }
        }
    }
}
