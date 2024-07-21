using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Captcha
{
    public interface IMathCaptcha
    {
        MathCaptchaResult Generate();
        MathCaptchaValidationResult IsValid(string id, int answer);
    }
}
