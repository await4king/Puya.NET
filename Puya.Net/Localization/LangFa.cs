using System;
using System.Collections.Generic;
using System.Text;
using Puya.Conversion;

namespace Puya.Localization
{
    public class LangFa : Language
    {
        public LangFa()
        {
            this.Type = LangType.fa;
            this.Name = "Farsi";
            this.Culture = "Persian";
            this.AltName = "Persian";
            this.LocalName = "فارسي";
            this.ShortName = "fa";
            this.DigitSeparator = '،';
            this.Direction = TextDirection.rtl;
            this.AltDirection = TextDirection.ltr;
            this.Align = TextAlign.right;
            this.AltAlign = TextAlign.left;
            this.Digits = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }
        public override string Render(object text)
        {
            var result = "";

            foreach (var ch in SafeClrConvert.ToString(text))
            {
                if (Char.IsDigit(ch) && ((int)ch - 48) < 10)
                    //result += "&#" + (1632 + ((int)ch - 48)).ToString() + ";";
                    result += this.Digits[(int)ch - 48];
                else
                    result += ch;
            }

            return result;
        }
    }
}
