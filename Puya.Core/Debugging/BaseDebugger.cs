using Puya.Debugging;
using System;
using System.Linq;

namespace Puya.Core.Debugging
{
    public abstract class BaseDebugger : IDebugger
    {
        public DebuggerOptions Options { get; set; }
        public BaseDebugger(DebuggerOptions options)
        {
            Options = options;
        }
        protected virtual bool? GetIsDebugging()
        {
            return null;
        }
        protected abstract string GetUserName();
        protected abstract bool IsInRole(string roleName);
        private bool _isDebugging;
        public virtual bool IsDebugging
        {
            get
            {
                if (Options != null)
                {
                    var debuggers = Options.DebuggerUsers?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()) ?? new string[] { };
                    var username = GetUserName();
                    var isDebugging = GetIsDebugging();

                    _isDebugging = Options.DebuggingEnabled
                                        &&
                                        (isDebugging == null || isDebugging.Value)
                                        &&
                                        (
                                            Options.GlobalDebugging
                                                ||
                                            IsInRole(Options.DebuggerRoleName)
                                                ||
                                            debuggers.Contains(username, StringComparer.OrdinalIgnoreCase)
                                        );
                }

                return _isDebugging;
            }
            set
            {
                _isDebugging = value;
            }
        }
    }
}
