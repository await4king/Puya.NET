using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Extensions
{
    public static class ExceptionExtentions
    {
        public static string ToString(this Exception e, string separator)
        {
            var sb = new StringBuilder();

            if (e != null)
            {
                sb.Append(e.Message);

                var innerException = e.InnerException;

                while (innerException != null)
                {
                    sb.Append(separator + innerException.Message);

                    innerException = innerException.InnerException;
                }
            }

            return sb.ToString();
        }
        public static string ToString(this Exception e, char separator)
        {
            return e.ToString(separator.ToString());
        }
    }
}
