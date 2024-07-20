using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Puya.Configuration
{
    public class SpacerAttribute : Attribute
    { }
    public class DataStoreConfigItem
    {

        #region properties

        private string _Driver;

        public string Driver { get { return _Driver; } set { _Driver = value; _value = null; } }


        private string _Provider;

        public string Provider { get { return _Provider; } set { _Provider = value; _value = null; } }


        private string _Server;

        public string Server { get { return _Server; } set { _Server = value; _value = null; } }


        private string _Database;

        public string Database { get { return _Database; } set { _Database = value; _value = null; } }


        private string _DataProvider;
        [Spacer]
        public string DataProvider { get { return _DataProvider; } set { _DataProvider = value; _value = null; } }

        private string _Credentials;

        public string Credentials { get { return _Credentials; } set { _Credentials = value; _value = null; } }

        private string _Value;

        public string Value { get { return _Value; } set { _Value = value; _value = null; } }

        private string _UserId;
        [Spacer]
        public string UserId { get { return _UserId; } set { _UserId = value; _value = null; } }


        private string _Password;

        public string Password { get { return _Password; } set { _Password = value; _value = null; } }


        private string _UID;

        public string UID { get { return _UID; } set { _UID = value; _value = null; } }


        private string _PWD;

        public string PWD { get { return _PWD; } set { _PWD = value; _value = null; } }


        private bool? _MultipleActiveResultSets;

        public bool? MultipleActiveResultSets { get { return _MultipleActiveResultSets; } set { _MultipleActiveResultSets = value; _value = null; } }


        private bool? _MARSConnection;
        [Spacer]
        public bool? MARSConnection { get { return _MARSConnection; } set { _MARSConnection = value; _value = null; } }


        private string _MARS_Connection;

        public string MARS_Connection { get { return _MARS_Connection; } set { _MARS_Connection = value; _value = null; } }


        private string _Encrypt;

        public string Encrypt { get { return _Encrypt; } set { _Encrypt = value; _value = null; } }


        private string _Trusted_Connection;

        public string Trusted_Connection { get { return _Trusted_Connection; } set { _Trusted_Connection = value; _value = null; } }


        private string _NetworkLibrary;
        [Spacer]
        public string NetworkLibrary { get { return _NetworkLibrary; } set { _NetworkLibrary = value; _value = null; } }


        private string _InitialCatalog;
        [Spacer]
        public string InitialCatalog { get { return _InitialCatalog; } set { _InitialCatalog = value; _value = null; } }


        private string _AttachDbFilename;

        public string AttachDbFilename { get { return _AttachDbFilename; } set { _AttachDbFilename = value; _value = null; } }


        private string _IntegratedSecurity;
        [Spacer]
        public string IntegratedSecurity { get { return _IntegratedSecurity; } set { _IntegratedSecurity = value; _value = null; } }


        private string _DataSource;
        [Spacer]
        public string DataSource { get { return _DataSource; } set { _DataSource = value; _value = null; } }


        private bool? _AsynchronousProcessing;
        [Spacer]
        public bool? AsynchronousProcessing { get { return _AsynchronousProcessing; } set { _AsynchronousProcessing = value; _value = null; } }


        private int? _PacketSize;
        [Spacer]
        public int? PacketSize { get { return _PacketSize; } set { _PacketSize = value; _value = null; } }


        private string _ColumnEncryptionSetting;
        [Spacer]
        public string ColumnEncryptionSetting { get { return _ColumnEncryptionSetting; } set { _ColumnEncryptionSetting = value; _value = null; } }


        private string _EnclaveAttestationUrl;
        [Spacer]
        public string EnclaveAttestationUrl { get { return _EnclaveAttestationUrl; } set { _EnclaveAttestationUrl = value; _value = null; } }


        private int? _ConnectTimeout;
        [Spacer]
        public int? ConnectTimeout { get { return _ConnectTimeout; } set { _ConnectTimeout = value; _value = null; } }


        private string _MultiSubnetFailover;

        public string MultiSubnetFailover { get { return _MultiSubnetFailover; } set { _MultiSubnetFailover = value; _value = null; } }


        private string _ApplicationIntent;

        public string ApplicationIntent { get { return _ApplicationIntent; } set { _ApplicationIntent = value; _value = null; } }


        private string _FailoverPartner;
        [Spacer]
        public string FailoverPartner { get { return _FailoverPartner; } set { _FailoverPartner = value; _value = null; } }


        private string _Failover_Partner;

        public string Failover_Partner { get { return _Failover_Partner; } set { _Failover_Partner = value; _value = null; } }



        #endregion
        public string Name { get; set; }
        private string _value;
        public string GetConnectionString(Func<string, string> decryptor)
        {
            return GetConnectionString(dsi =>
            {
                if (!string.IsNullOrEmpty(dsi.Value))
                {
                    try
                    {
                        var decrypted = decryptor(dsi.Value);
                        var items = JsonConvert.DeserializeObject<Dictionary<string, object>>(decrypted);

                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                var prop = dsi.GetType().GetProperty(item.Key);

                                if (prop != null)
                                {
                                    prop.SetValue(dsi, item.Value);
                                }
                            }
                        }
                    }
                    catch
                    { }
                }
            });
        }
        public string GetConnectionString(Action<DataStoreConfigItem> decryptor = null)
        {
            if (string.IsNullOrEmpty(_value))
            {
                var sb = new StringBuilder();

                decryptor?.Invoke(this);

                foreach (var prop in this.GetType().GetProperties())
                {
                    var propName = prop.Name;

                    if (propName == "Name" || propName == "Value" || propName == "Credentials")
                    {
                        continue;
                    }

                    var propValue = prop.GetValue(this)?.ToString();

                    if (prop.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(SpacerAttribute)) != null)
                    {
                        var name = "";

                        foreach (var ch in propName)
                        {
                            name += Char.IsUpper(ch) ? " " + ch : ch.ToString();
                        }

                        propName = name.Trim();
                    }

                    if (!string.IsNullOrEmpty(propValue))
                    {
                        sb.Append($"{propName}={propValue};");
                    }
                }

                _value = sb.ToString();
            }

            return _value;
        }
    }
    public class DataStoreConfig : List<DataStoreConfigItem>
    {
        public string GetConnectionString(string name, Action<DataStoreConfigItem> decryptor = null)
        {
            var result = "";

            foreach (var dsi in this)
            {
                if (string.Compare(dsi.Name, name, StringComparison.InvariantCulture) == 0)
                {
                    result = dsi.GetConnectionString(decryptor);
                    break;
                }
            }

            return result;
        }
        public string GetConnectionString(string name, Func<string, string> decryptor)
        {
            var result = "";

            foreach (var dsi in this)
            {
                if (string.Compare(dsi.Name, name, StringComparison.InvariantCulture) == 0)
                {
                    result = dsi.GetConnectionString(decryptor);
                    break;
                }
            }

            return result;
        }
    }
}
