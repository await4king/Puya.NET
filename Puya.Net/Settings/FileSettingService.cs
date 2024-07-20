using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Settings
{
    public abstract class FileSettingService: InMemorySettingService
    {
        public string Path { get; set; }
        public bool AutoSave { get; set; }
        public FileSettingService(string path, bool autoSave)
        {
            Path = path;
            AutoSave = autoSave;
        }
        public void Load()
        {
            _items.Clear();
            var autoSave = AutoSave;

            AutoSave = false;

            LoadInternal();

            AutoSave = autoSave;
        }
        protected abstract void LoadInternal();
        protected abstract bool Save();
        protected virtual string GetInternal(string key)
        {
            return base.Get(key);
        }
        public override string Get(string key)
        {
            return GetInternal(key);
        }
        protected virtual bool SetInternal(string key, string value)
        {
            return base.Set(key, value);
        }
        public override bool Set(string key, string value)
        {
            var result = SetInternal(key, value);

            if (result && AutoSave)
            {
                result = Save();
            }

            return result;
        }
        public override Task<bool> SetAsync(string key, string value, CancellationToken cancellation)
        {
            var result = Set(key, value);

            return Task.FromResult(result);
        }
    }
}
