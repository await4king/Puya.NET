using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Date
{
    public interface INow
    {
        DateTime Value { get; }
    }
    public class DateTimeNow : INow
    {
        public DateTime Value
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
    public class DateTimeUtcNow : INow
    {
        public DateTime Value
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
