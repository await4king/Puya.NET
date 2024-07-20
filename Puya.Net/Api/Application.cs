using Puya.Collections;

namespace Puya.Api
{
    public class ApplicationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string Version { get; set; }
        public string Code { get; set; }
        public string BasePath { get; set; }
        public bool Disabled { get; set; }
        public string Settings { get; set; }
    }
    public class Application: ISettingable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string Version { get; set; }
        public string Code { get; set; }
        public string BasePath { get; set; }
        public bool Disabled { get; set; }
        private KeyValueSettings settings;
        public KeyValueSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new KeyValueSettings();
                }

                return settings;
            }
            set { settings = value; }
        }
    }
}
