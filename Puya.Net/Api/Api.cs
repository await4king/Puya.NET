using System.Collections.Generic;
using Puya.Collections;

namespace Puya.Api
{
    public class ApiModel
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public string Service { get; set; }
        public string Action { get; set; }
        public bool Disabled { get; set; }
        public bool Async { get; set; }
        public string Apps { get; set; }
        public string HttpVerbs { get; set; }
        public string Settings { get; set; }
    }
    public class Api: ISettingable
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public string Service { get; set; }
        public string Action { get; set; }
        public bool Disabled { get; set; }
        public bool Async { get; set; }
        private List<int> apps;
        public List<int> Apps
        {
            get
            {
                if (apps == null)
                {
                    apps = new List<int>();
                }

                return apps;
            }
            set { apps = value; }
        }
        public string HttpVerbs { get; set; }
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
