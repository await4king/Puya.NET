using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.Sms
{
    public class AssemblySmsService : SmsServiceBase<AssemblySmsServiceConfig>
    {
        public AssemblySmsService(AssemblySmsServiceConfig config, ISmsLogger logger) : base(config, logger)
        { }
        public AssemblySmsService(ISmsLogger logger) : base(logger)
        { }
        public AssemblySmsService(AssemblySmsServiceConfig config) : base(config)
        { }
        public AssemblySmsService()
        { }

        ISmsService _service;
        public virtual ISmsService GetService()
        {
            if (_service == null && !string.IsNullOrEmpty(Config?.AssemblyName))
            {
                var name = Config.AssemblyName;

                if (!name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    name = name + ".dll";
                }

                var assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

                if (File.Exists(assemblyPath))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(assemblyPath);
                        var serviceTypes = assembly.GetTypes().Where(type => type.Implements<ISmsService>());

                        if (serviceTypes.Count() > 0)
                        {
                            var found = false;

                            foreach (var serviceType in serviceTypes)
                            {
                                var serviceConfigType = assembly.GetTypes().First(type => type.DescendsFrom<SmsConfigItem>());
                                var smsConfig = (object)null;

                                if (serviceConfigType != null)
                                {
                                    smsConfig = Config.InnerConfig.ToStrongConfig(serviceConfigType, (e, prop) =>
                                    {
                                        Danger(e, $"Error reading Sms service config prop {prop}");
                                    });
                                }

                                var ctors = serviceType.GetConstructors();
                                var _ctor = null as ConstructorInfo;
                                var parameters = null as ParameterInfo[];
                                var args = null as object[];

                                do
                                {
                                    // first choice: ctor(TConfig, ISmsLogger) where TConfig: SmsConfigItem

                                    foreach (var ctor in ctors.Where(c => c.GetParameters().Length == 2))
                                    {
                                        parameters = ctor.GetParameters();

                                        if (parameters[0].ParameterType.DescendsFrom<SmsConfigItem>() && parameters[1].ParameterType.Implements<ISmsLogger>())
                                        {
                                            _ctor = ctor;
                                            args = new object[] { smsConfig, Logger };
                                            break;
                                        }

                                        if (parameters[1].ParameterType.DescendsFrom<SmsConfigItem>() && parameters[0].ParameterType.Implements<ISmsLogger>())
                                        {
                                            _ctor = ctor;
                                            args = new object[] { Logger, smsConfig };
                                            break;
                                        }
                                    }

                                    if (_ctor != null)
                                    {
                                        break;
                                    }

                                    // second choice: ctor(TConfig) where TConfig: SmsConfigItem
                                    //                  or
                                    //                ctor(ISmsLogger)

                                    foreach (var ctor in ctors.Where(c => c.GetParameters().Length == 1))
                                    {
                                        parameters = ctor.GetParameters();

                                        if (parameters[0].ParameterType.DescendsFrom<SmsConfigItem>())
                                        {
                                            _ctor = ctor;
                                            args = new object[] { smsConfig };
                                            break;
                                        }

                                        if (parameters[0].ParameterType.Implements<ISmsLogger>())
                                        {
                                            _ctor = ctor;
                                            args = new object[] { Logger };
                                            break;
                                        }
                                    }

                                    if (_ctor != null)
                                    {
                                        break;
                                    }

                                    // last choice: default constructor ctor()

                                    foreach (var ctor in ctors.Where(c => c.GetParameters().Length == 0))
                                    {
                                        _ctor = ctor;
                                        args = new object[] { };
                                        break;
                                    }
                                } while (false);

                                if (_ctor == null)
                                {
                                    Debug($"{serviceType.Name} implements ISmsService, but does not have a suitable constructor..");

                                    continue;
                                }
                                else
                                {
                                    Debug($"found {serviceType.Name} type. Trying to instantiate from it ...");
                                }

                                try
                                {
                                    _service = (ISmsService)_ctor.Invoke(args);

                                    found = true;

                                    break;
                                }
                                catch (Exception e)
                                {
                                    Danger(e, $"Error instantiating from {serviceType.Name}");
                                }
                            }

                            if (!found)
                            {
                                Debug($"No class found in {name} assembly that implements 'ISmsService' with a suitable constructor. Cannot use {name} assembly.");
                            }
                        }
                        else
                        {
                            Warn($"Assembly {Config.AssemblyName} does not contain a class that implements Puya.Sms.ISmsService");
                        }
                    }
                    catch (Exception e)
                    {
                        Danger(e, $"Loading assembly failed: {assemblyPath}");
                    }
                }
                else
                {
                    Warn($"Assembly not found: {assemblyPath}");
                }
            }

            return _service;
        }
        protected override SendResponse SendInternal(string mobile, string message)
        {
            var result = null as SendResponse;
            var service = GetService();

            if (service == null)
            {
                result = new SendResponse();

                result.SetStatus("NoService");
            }
            else
            {
                result = service.Send(mobile, message);
            }

            return result;
        }
        protected override async Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation)
        {
            var result = null as SendResponse;
            var service = GetService();

            if (service == null)
            {
                result = new SendResponse();

                result.SetStatus("NoService");
            }
            else
            {
                result = await service.SendAsync(mobile, message, cancellation);
            }

            return result;
        }
    }
}
