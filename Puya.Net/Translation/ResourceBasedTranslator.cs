using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Caching;
using Puya.Extensions;
using Puya.Localization;
using Puya.Logging;

namespace Puya.Translation
{
    public class ResourceBasedTranslator : BaseTranslator
    {
        public ResourceBasedTranslator() : this(null, null)
        { }
        public ResourceBasedTranslator(ICache cache) : this(cache, null)
        { }
        public ResourceBasedTranslator(ILogger logger) : this(null, logger)
        { }
        public ResourceBasedTranslator(ICache cache, ILogger logger) : this(cache, logger, null)
        { }
        public ResourceBasedTranslator(ICache cache, ILogger logger, ILanguageProvider languageProvider) : base(cache, logger, languageProvider)
        {
            Init();
        }

        protected virtual void Init()
        {
            Assemblies = new List<string>();

            //Assemblies.Add("Puya.Language");
            //Assemblies.Add("Puya.Measurement");
            //Assemblies.Add("Puya.Calendar");
            // //Assemblies.Add("Puya.Translation");
        }
        public List<string> Assemblies { get; private set; }
        private string GetFileName(string resourcename)
        {
            var result = "";
            var i = resourcename.LastIndexOf('.');

            if (i > 1)
            {
                var j = resourcename.LastIndexOf('.', i - 1);

                if (j >= 0)
                {
                    result = resourcename.Substring(j + 1, i - j - 1);
                }
            }

            return result?.ToLower();
        }
        protected virtual string GetCategoryOf(string resource)
        {
            return GetFileName(resource);
        }
        private void LoadAssemblyTexts(Assembly assembly)
        {
            Logger.Info($"LoadAssemblyTexts: {assembly.GetName().Name}");

            var resources = assembly.GetManifestResourceNames();

            if (resources.Length > 0)
            {
                var cdt = resources.Where(rn => rn.ToLower().EndsWith(Options.CultureDependentTextExtension));

                Logger.Debug($"found {cdt.Count()} culture dependent resources ");

                foreach (var rn in cdt)
                {
                    var content = assembly.GetResourceString(rn.Substring(assembly.GetName().Name.Length + 1));

                    Logger.Debug($"Loading Resource: {rn}");

                    try
                    {
                        var filename = GetFileName(rn);
                        var category = GetCategoryOf(rn);

                        Logger.Debug($"filename: {filename}");

                        ProcessCultureDependentText(category, content);
                    }
                    catch (Exception e)
                    {
                        Logger.Debug($"error loading {rn}");

                        Logger.Danger(e, Name, new { resource = rn });
                    }
                }

                var cit = resources.Where(rn => rn.ToLower().EndsWith(Options.CultureIndependentTextExtension));

                Logger.Debug($"found {cit.Count()} culture independent resources ");

                foreach (var rn in cit)
                {
                    var content = assembly.GetResourceString(rn.Substring(assembly.GetName().Name.Length + 1));

                    Logger.Debug($"Loading Resource: {rn}");

                    try
                    {
                        var filename = GetFileName(rn);
                        var category = GetCategoryOf(rn);

                        Logger.Debug($"filename: {filename}");

                        ProcessCultureIndependentText(category, content);
                    }
                    catch (Exception e)
                    {
                        Logger.Debug($"error loading {rn}");

                        Logger.Danger(e, Name, new { resource = rn });
                    }
                }
            }
            else
            {
                Logger.Debug($"no resource file found to read.");
            }
        }
        protected override void LoadInternal()
        {
            var assemblies = new List<Assembly>();

            Logger.Info($"{this.GetType().Name}.LoadInternal");
            Logger.Debug("Current resource assemblies : ", logger => Assemblies.Join(", "));

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            Logger.Debug("Checking if an assembly is already loaded ...");

            loadedAssemblies.ForEach(asm => {
                if (Assemblies.IndexOf(asm.GetName().Name) > 0)
                {
                    Logger.Debug("already loaded");

                    assemblies.Add(asm);
                }
            });

            if (assemblies.Count < Assemblies.Count)
            {
                Logger.Debug("Some assemblies are not loaded. Trying to load them ...");
                Logger.Debug("Finding their paths ...");

                var loadedPaths = loadedAssemblies.Where(a => !a.IsDynamic).Select(a => a.Location).ToArray();
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (baseDirectory.Right(1) == "\\")
                {
                    baseDirectory = baseDirectory.Left(baseDirectory.Length - 1);
                }

                var referencedPaths = Directory.GetFiles(baseDirectory, "*.dll").ToList();

                if (baseDirectory.Right(3).ToLower() != "\\bin" && Directory.Exists(Path.Combine(baseDirectory, "bin")))
                {
                    referencedPaths.Merge(Directory.GetFiles(Path.Combine(baseDirectory, "bin"), "*.dll"));
                }

                var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
                var finalLoad = new List<string>();

                if (toLoad.Count > 0)
                {
                    Logger.Debug($" {toLoad.Count} assemblies were not loaded. Checking if our resource assemblies are in them ...");

                    foreach (var path in toLoad)
                    {
                        foreach (var assembly in Assemblies)
                        {
                            if (string.Compare(Path.GetFileName(path).Left(assembly.Length), assembly, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                Logger.Debug($"{assembly} was added to final load list.");

                                finalLoad.Add(path);

                                break;
                            }
                        }
                    }

                    if (finalLoad.Count > 0)
                    {
                        Logger.Debug("Loading not-loaded assemblies ...");

                        finalLoad.ForEach(path =>
                        {
                            Logger.Debug($"Loading {path} ...");

                            var asm = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path));

                            if (assemblies.IndexOf(asm) < 0)
                            {
                                assemblies.Add(asm);
                            }

                            Logger.Debug($"{asm.GetName().Name} Loaded.");
                        });
                    }
                    else
                    {
                        Logger.Debug("Nothing found to load.");
                    }
                }
                else
                {
                    Logger.Debug("All assemblies in '\\bin' folder are already loaded.");
                }
            }

            if (assemblies.Count > 0)
            {
                Logger.Debug("Loading resources");

                assemblies.ForEach(asm =>
                {
                    Logger.Debug($"Loading {asm.GetName().Name} texts ...");

                    LoadAssemblyTexts(asm);

                    Logger.Debug($"Loaded.");
                });
            }
        }
    }
}
