using Puya.Core.Debugging;
using System;
using System.Linq;

namespace Puya.Debugging
{
    public class ManualDebugger : BaseDebugger
    {
        string _username;
        string _roles;

        public ManualDebugger() : this(new DebuggerOptions())
        { }
        public ManualDebugger(DebuggerOptions options) : base(options)
        {
        }

        protected override string GetUserName()
        {
            return _username;
        }
        public void SetUserName(string username)
        {
            _username = username;
        }
        public void SetRoles(string roles)
        {
            _roles = roles;
        }
        protected override bool IsInRole(string roleName)
        {
            var roles = _roles?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()) ?? new string[] { };

            return !string.IsNullOrEmpty(_username) && roles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
