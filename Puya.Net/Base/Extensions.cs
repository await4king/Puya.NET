using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Base
{
    public static class Extensions
    {
        public static T Activate<T>(this IObjectActivator activator, params object[] args)
        {
            return (T)activator.Activate(typeof(T), args);
        }
    }
}
