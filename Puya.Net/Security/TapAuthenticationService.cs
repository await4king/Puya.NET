using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Security
{
    public interface ITapAuthenticationHelper
    {
        string EncodePassword(string password);
    }
    public class TapAuthenticationHelper : ITapAuthenticationHelper
    {
        public string EncodePassword(string password)
        {
            return TapAuthenticationService.EncodePassword(password);
        }
    }
    public class TapAuthenticationService
    {
        public static string EncodePassword(string password)
        {
            return CheckCode(password);
        }
        private static string CheckCode(string InpStr)
        {
            var TempArr = new string[72];
            string K;
            int i, L;
            int X1, X2, X3;
            int Y, X;
            string Y1, Y2, Y3;
            string Result = "";

            try
            {
                X1 = 0;
                X2 = 0;
                X3 = 0;
                L = InpStr.Length;

                if (L > 72)
                {
                    InpStr = InpStr.Substring(0, 72);
                }

                L = InpStr.Length;

                for (i = 0; i < L; i++)
                {
                    K = ((int)(InpStr[i])).ToString();
                    if (K.Length == 1) K = "00" + K;
                    if (K.Length == 2) K = "0" + K;
                    TempArr[i] = K;
                    X1 = X1 * (i + 1) + Int32.Parse(K[2].ToString());
                    X2 = X2 * (i + 1) + Int32.Parse(K[1].ToString());
                    X3 = X3 * (i + 1) + Int32.Parse(K[0].ToString());
                    if (X3 == 0) X3 = 1;
                }
                X = X1 + X2 + X3 + L;
                Y1 = X1.ToString()[0].ToString();
                Y2 = X2.ToString()[0].ToString();
                Y3 = X3.ToString()[0].ToString();

                if (Y3 == "0") Y3 = "1";
                Y = Int32.Parse(Y1 + Y2 + Y3) + L;
                Result = (X * Y * (X + Y)).ToString();
            }
            catch
            { }

            return Result;
        }
    }
}
