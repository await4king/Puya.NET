using System;
using System.Collections.Generic;
using System.Text;
using Puya.Conversion;

namespace Puya.Localization
{
    public class LangEn : Language
    {
        public LangEn()
        {
            this.Type = LangType.en;
            this.Name = "English";
            this.Culture = "Gregorian";
            this.AltName = "English";
            this.LocalName = "English";
            this.ShortName = "en";
            this.DigitSeparator = ',';
            this.Direction = TextDirection.ltr;
            this.AltDirection = TextDirection.rtl;
            this.Align = TextAlign.left;
            this.AltAlign = TextAlign.right;
            this.Digits = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }
        public override string Render(object text)
        {
            return SafeClrConvert.ToString(text);
        }
    }
}
