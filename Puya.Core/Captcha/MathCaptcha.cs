using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Captcha
{
    public class MathCaptcha : IMathCaptcha
    {
        private MathCaptchaConfig config;
        public MathCaptchaConfig Config
        {
            get
            {
                if (config == null)
                    config = new MathCaptchaConfig();

                return config;
            }
            set { config = value; }
        }
        private IMathCaptchaStore store;
        public IMathCaptchaStore Store
        {
            get
            {
                if (store == null)
                    store = new InMemoryMathCaptchaStore();

                return store;
            }
            set { store = value; }
        }
        public MathCaptcha(): this(null, null)
        { }
        public MathCaptcha(MathCaptchaConfig config, IMathCaptchaStore store)
        {
            Config = config;
            Store = store;
        }
        public MathCaptchaResult Generate()
        {
            var rand = new Random();
            var item = new MathCaptchaItem
            {
                Id = Guid.NewGuid().ToString(),
                A = rand.Next(Config.MinA, Config.MaxA),
                B = rand.Next(Config.MinB, Config.MaxB),
            };

            if (rand.Next(0, 10) >= 5)
            {
                var temp = item.A;
                item.A = item.B;
                item.B = temp;
            }

            item = Store.GetOrAdd(item.Id, item);

            return new MathCaptchaResult { Id = item.Id, A = item.A, B = item.B };
        }

        public MathCaptchaValidationResult IsValid(string id, int answer)
        {
            MathCaptchaValidationResult result;
            MathCaptchaItem item;

            if (Store.TryGetValue(id, out item))
            {
                if (item.AttemptCount < Config.MaxAttempts)
                {
                    if (item.A + item.B == answer)
                    {
                        result = MathCaptchaValidationResult.Valid;
                    }
                    else
                    {
                        result = MathCaptchaValidationResult.Invalid;

                        Store.AddOrUpdate(item.Id, item.Challenged());
                    }
                }
                else
                {
                    if ((DateTime.Now - item.LastAttempt).Seconds > Config.LockDuration)
                    {
                        Store.AddOrUpdate(item.Id, item.Reset());

                        result = IsValid(id, answer);
                    }
                    else
                    {
                        result = MathCaptchaValidationResult.Locked;
                    }
                }
            }
            else
            {
                result = MathCaptchaValidationResult.NotFound;
            }

            return result;
        }
    }
}
