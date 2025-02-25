using Puya.Base;
using Puya.Collections;
using System.Collections.Generic;

namespace Puya.Service
{
    public abstract class BaseActionBasedService<TConfig>: IService<TConfig>
        where TConfig : class, IServiceConfig, new()
    {
        private string name;
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    name = this.GetType().Name;
                
                return name;
            }
        }
        private TConfig _config;
        public TConfig Config
        {
            get
            {
                return TypeHelper.EnsureInitialized<TConfig, TConfig>(ref _config);
            }
            set { _config = value; }
        }
        public virtual IServiceAction GetAction(string name)
        {
            return Actions[name];
        }
        public IDictionary<string, IServiceAction> Actions { get; private set; }
        public BaseActionBasedService(): this(null)
        { }
        public BaseActionBasedService(TConfig config)
        {
            Config = config;
            Actions = new CaseSensitiveDictionary<IServiceAction>();
        }
        public IServiceAction this[string action]
        {
            get
            {
                return Actions[action];
            }
        }
    }
}
