using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Text
{
    public class NumberEn : Number
    {
        public NumberEn(int n) : base(n, "en")
        {
            zero = "zero";
            and = " and ";
            and2 = " ";
            and3 = " and ";
            ones = new string[] { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            teens = new string[] { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            tens = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            hundreds = new string[] { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred" };
            units = new string[] { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion" };
        }
    }
}
