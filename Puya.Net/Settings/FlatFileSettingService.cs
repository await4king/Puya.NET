using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Settings
{
    public class FlatFileSettingService: FileSettingService
    {
        public char KeyValueSeparator { get; set; }
        public string CommentIndicator { get; set; }
        public FlatFileSettingService(string path, bool autoSave = true, char keyValueSeparator = ':', string commentIndicator = "#"): base(path, autoSave)
        {
            KeyValueSeparator = keyValueSeparator;
            CommentIndicator = commentIndicator;

            Load();
        }
        public override int Count => GetAll().Count;
        protected override void LoadInternal()
        {
            var lines = File.ReadAllLines(Path);
            var count = 0;

            foreach (var line in lines)
            {
                count++;

                // It is not a correct practice to manipulate _items directly.
                // _items should only be set using Set() method.
                // However, we safely do this.
                // we overrided SetInternal() to prevent overwriting comment and new line items by the user.
                // we needed some way to add these items in the _items dictionary while loading.
                // There was no easier option to do this, but directly writing them in _items.
                // This is not bad that much, because we are in LoadInternal().
                // This method is called only once while loading settings.

                if (string.IsNullOrEmpty(line))
                {
                    _items[$"line:{count}"] = "";

                    continue;
                }

                if (line.StartsWith(CommentIndicator))
                {
                    _items[$"comment:{count}${line}"] = "";

                    continue;
                }

                var separatorIndex = line.IndexOf(KeyValueSeparator);

                if (separatorIndex > 0)
                {
                    var key = line.Substring(0, separatorIndex);
                    var value = line.Substring(separatorIndex + 1).Trim();

                    Set(key, value);
                }
            }
        }
        protected override bool Save()
        {
            var result = false;
            var content = new StringBuilder();

            try
            {
                foreach (var item in _items)
                {
                    var noKeyValueSeparator = false;
                    var key = item.Key;
                    
                    if (key.StartsWith("line:"))
                    {
                        key = "";
                        noKeyValueSeparator = true;
                    }
                    else
                    {
                        if (key.StartsWith("comment:"))
                        {
                            var dollarIndex = key.IndexOf("$");

                            key = key.Substring(dollarIndex + 1);
                            noKeyValueSeparator = true;
                        }
                    }

                    content.Append($"{key}{(noKeyValueSeparator ? "": $"{KeyValueSeparator} {item.Value}")}{Environment.NewLine}");
                }

                File.WriteAllText(Path, content.ToString());

                result = true;
            }
            catch
            { }

            return result;
        }
        protected bool IsValidKey(string key)
        {
            return !(string.IsNullOrEmpty(key) || key.StartsWith("line:") || key.StartsWith("comment:"));
        }
        public override IDictionary<string, string> GetAll()
        {
            return base.GetAll().Where(item => IsValidKey(item.Key)).ToDictionary(x => x.Key, x => x.Value);
        }
        public override Task<IDictionary<string, string>> GetAllAsync(CancellationToken cancellation)
        {
            var items = GetAll();

            return Task.FromResult(items);
        }
        protected override string GetInternal(string key)
        {
            if (!IsValidKey(key))
                return "";

            return base.GetInternal(key);
        }
        protected override bool SetInternal(string key, string value)
        {
            if (!IsValidKey(key))
                return false;

            return base.SetInternal(key, value);
        }
    }
}
