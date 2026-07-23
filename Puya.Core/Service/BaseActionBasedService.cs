using Puya.Collections;
using System.Collections.Generic;

namespace Puya.Service
{
    public abstract class BaseActionBasedService: IService
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
            set { name = value; }
        }
        public virtual IServiceAction GetAction(string name)
        {
            return Actions[name];
        }
        public IDictionary<string, IServiceAction> Actions { get; private set; }
        public BaseActionBasedService()
        {
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
