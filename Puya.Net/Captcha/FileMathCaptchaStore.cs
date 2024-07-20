using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Puya.Captcha
{
    public class FileMathCaptchaStore : IMathCaptchaStore
    {
        private Dictionary<string, MathCaptchaItem> store;
        public bool UseLock { get; set; }
        public string FileName { get; set; }
        public FileMathCaptchaStore() : this("")
        {
        }
        public FileMathCaptchaStore(string filename)
        {
            Init(filename);
        }
        private void Init(string filename = "")
        {
            store = new Dictionary<string, MathCaptchaItem>();
            FileName = filename;

            if (string.IsNullOrEmpty(FileName))
            {
                FileName = "captcha.math.json";
            }

            Load();
        }
        private void Load()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Path.Combine(path, FileName);

            var content = File.ReadAllText(path);

            store = JsonConvert.DeserializeObject<Dictionary<string, MathCaptchaItem>>(content);
        }
        private void Save()
        {
            var content = JsonConvert.SerializeObject(store, Formatting.Indented);
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Path.Combine(path, FileName);

            File.WriteAllText(path, content);
        }
        public MathCaptchaItem GetOrAdd(string id, MathCaptchaItem item)
        {
            if (store.ContainsKey(id))
            {
                return store[id];
            }
            else
            {
                store.Add(id, item);

                Save();

                return item;
            }
        }

        public bool TryGetValue(string id, out MathCaptchaItem item)
        {
            return store.TryGetValue(id, out item);
        }

        public void AddOrUpdate(string id, MathCaptchaItem item)
        {
            if (store.ContainsKey(id))
            {
                store[id] = item;
            }
            else
            {
                store.Add(id, item);
            }

            Save();
        }
    }
}
