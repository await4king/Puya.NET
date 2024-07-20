using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Captcha
{
    public interface IMathCaptchaStore
    {
        MathCaptchaItem GetOrAdd(string id, MathCaptchaItem item);
        bool TryGetValue(string id, out MathCaptchaItem item);
        void AddOrUpdate(string id, MathCaptchaItem item);
    }
}
