using Puya.Collections;
using System;

namespace Puya.Data
{
    public class DefaultConnectionStringProvider : IConnectionStringProvider
    {
        private string currentName;
        private DynamicModel<string> connectionStrings;
        public static string NoNamed = "NO_NAMED";
        public DefaultConnectionStringProvider()
        {
            connectionStrings = new DynamicModel<string>();
            currentName = NoNamed;
        }
        public int Count
        {
            get
            {
                return connectionStrings.Count;
            }
        }

        public string GetConnectionString()
        {
            return GetConnectionString(currentName);
        }

        public string GetConnectionString(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (connectionStrings.ContainsKey(name))
            {
                return connectionStrings[name];
            }

            return string.Empty;
        }

        public void SetConnectionString(string constr)
        {
            SetConnectionString(currentName, constr);
        }

        public void SetConnectionString(string name, string constr)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            connectionStrings[name] = constr;
        }

        public void SetCurrent(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (connectionStrings.ContainsKey(name))
            {
                currentName = name;
            }
            else
            {
                throw new ApplicationException($"connectionString '{name}' was not found.");
            }
        }

        public string GetCurrent()
        {
            return currentName;
        }

        public void Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (connectionStrings.ContainsKey(name))
            {
                connectionStrings.Remove(name);
            }
            else
            {
                throw new ApplicationException($"connectionString '{name}' was not found.");
            }
        }
    }
}
