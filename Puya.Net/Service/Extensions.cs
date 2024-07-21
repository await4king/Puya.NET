using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Collections;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Service
{
    public static class Extensions
    {
        public static bool AddTapServices(this IServiceCollection services, params string[] assembliesToLoad)
        {
            var result = AssemblyLoader.Load(assembliesToLoad);

            var types = AppDomain.CurrentDomain.GetSubClasses<ServiceRegistery>(false).ToList();

            foreach (var type in types)
            {
                var sr = Activator.CreateInstance(type) as ServiceRegistery;

                foreach (var r in sr.Registery)
                {
                    switch (r.LifeTime)
                    {
                        case LifeTime.Transient:
                            services.AddTransient(r.Abstraction, r.Concretion);
                            break;
                        case LifeTime.Singleton:
                            services.AddSingleton(r.Abstraction, r.Concretion);
                            break;
                        case LifeTime.PerRequest:
                            services.AddScoped(r.Abstraction, r.Concretion);
                            break;
                    }
                }
            }

            return result;
        }
    }
}
