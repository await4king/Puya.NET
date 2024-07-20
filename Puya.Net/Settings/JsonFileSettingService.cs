using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Settings
{
    public class JsonFileSettingService : FileSettingService
    {
        public bool AutoFormat { get; set; }
        public JsonFileSettingService(string path, bool autoFormat = true, bool autoSave = true): base(path, autoSave)
        {
            AutoFormat = autoFormat;

            Load();
        }
        protected override void LoadInternal()
        {
            var content = File.ReadAllText(Path);

            if (!string.IsNullOrEmpty(content))
            {
                _items = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            }
        }
        protected override bool Save()
        {
            var content = JsonConvert.SerializeObject(_items, AutoFormat ? Formatting.Indented: Formatting.None);
            var result = false;

            try
            {
                File.WriteAllText(Path, content);

                result = true;
            }
            catch
            { }

            return result;
        }
    }
}
