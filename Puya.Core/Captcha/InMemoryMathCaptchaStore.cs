using System.Collections.Concurrent;

namespace Puya.Captcha
{
    public class InMemoryMathCaptchaStore : IMathCaptchaStore
    {
        private ConcurrentDictionary<string, MathCaptchaItem> store;
        public InMemoryMathCaptchaStore()
        {
            store = new ConcurrentDictionary<string, MathCaptchaItem>();
        }
        public MathCaptchaItem GetOrAdd(string id, MathCaptchaItem item)
        {
            return store.GetOrAdd(id, item);
        }

        public bool TryGetValue(string id, out MathCaptchaItem item)
        {
            return store.TryGetValue(id, out item);
        }

        public void AddOrUpdate(string id, MathCaptchaItem item)
        {
            store.AddOrUpdate(id, item, (key, old) => item);
        }
    }
}
