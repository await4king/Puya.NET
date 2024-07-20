using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Base
{
    public interface IObjectActivator
    {
        object Activate(Type type, params object[] args);
    }
    public class ObjectActivatorDefault : IObjectActivator
    {
        public object Activate(Type type, params object[] args)
        {
            var result = null as object;

            if (!type.IsInterface && !type.IsAbstract)
            {
                if (type == TypeHelper.TypeOfString && (args == null || args.Length == 0))
                {
                    result = Activator.CreateInstance(TypeHelper.TypeOfString, new char[] { });
                }
                else
                if (type.IsArray && (args == null || args.Length == 0))
                {
                    result = Activator.CreateInstance(type, 0);
                }
                else
                {
                    result = Activator.CreateInstance(type, args);
                }
            }

            return result;
        }
    }
    public class ObjectActivator: InstanceProvider<IObjectActivator, ObjectActivatorDefault>
    {
    }
}
