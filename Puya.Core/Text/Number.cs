using System;

namespace Puya.Text
{
    public abstract class Number
    {
        protected string zero;
        protected string and;
        protected string and2;
        protected string and3;
        protected string[] ones { get; set; }
        protected string[] teens { get; set; }
        protected string[] tens { get; set; }
        protected string[] hundreds { get; set; }
        protected string[] units { get; set; }
        protected long Value { get; set; }
        public string Lang { get; protected set; }
        public Number(long n, string lang)
        {
            Value = n;
            Lang = lang;
        }
        string wordify2(byte n)
        {
            var result = "";

            if (n < 10)
            {
                result = ones[n];
            }
            else if (n >= 10 && n < 20)
            {
                result = teens[n - 10];
            }
            else
            {
                var one = (byte)Math.Floor(n / 10d);
                var rest = (byte)(n - one * 10);

                result = tens[one - 2] + (rest > 0 ? and2 + ones[rest] : "");
            }

            return result;
        }
        string wordify3(short n)
        {
            var one = (byte)Math.Floor(n / 100d);
            var rest = (byte)(n - one * 100);

            return (one > 0 ? hundreds[one - 1] : "") + (rest > 0 ? (one > 0 ? and3 : "") + wordify2(rest) : "");
        }
        public virtual string Wordify()
        {
            var result = "";
            var str = Value.ToString();
            var numLength = str.Length;
            var i = 0;

            if (Value == 0)
            {
                result = zero;
            }
            else
            {
                do
                {
                    var from = numLength - (i + 1) * 3;
                    var len = 3;

                    if (from < 0)
                    {
                        len += from;
                        from = 0;
                    }

                    var num = str.Substring(from, len);

                    result = wordify3(Int16.Parse(num)) + " " + units[i] + (i < numLength && result.Length > 0 ? and + result : "");

                    i++;
                } while (i * 3 < numLength);
            }

            return result;
        }
    }
}
