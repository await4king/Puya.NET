using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puya.Base
{
    public static class AssemblyLoader
    {
        private static readonly ConcurrentDictionary<string, bool> AssemblyDirectories = new ConcurrentDictionary<string, bool>();
        static AssemblyLoader()
        {
            AssemblyDirectories[GetExecutingAssemblyDirectory()] = true;

            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }
        public static Assembly LoadWithDependencies(string assemblyPath)
        {
            AssemblyDirectories[Path.GetDirectoryName(assemblyPath)] = true;

            return Assembly.LoadFile(assemblyPath);
        }
        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var dependentAssemblyName = args.Name.Split(',')[0] + ".dll";
            var directoriesToScan = AssemblyDirectories.Keys.ToList();

            foreach (string directoryToScan in directoriesToScan)
            {
                var dependentAssemblyPath = Path.Combine(directoryToScan, dependentAssemblyName);

                if (File.Exists(dependentAssemblyPath))
                    return LoadWithDependencies(dependentAssemblyPath);
            }

            return null;
        }
        private static string GetExecutingAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path);
        }
        public static bool Load(params string[] assembliesToLoad)
        {
            // First trying to get all in above list, however this might not 
            // load all of them, because CLR will exclude the ones 
            // which are not used in the code

            var dataAssembliesNames =
               AppDomain.CurrentDomain.GetAssemblies()
                        .Where(assembly => assembliesToLoad.Any(a => assembly.GetName().Name == a))
                        .ToList();

            var loadedPaths = dataAssembliesNames.Select(a => a.Location).ToArray();

            var compareConfig = StringComparison.InvariantCultureIgnoreCase;
            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(f =>
                {
                    // filtering the ones which are in above list
                    var lastIndexOf = f.LastIndexOf("\\", compareConfig);
                    var dllIndex = f.LastIndexOf(".dll", compareConfig);

                    if (-1 == lastIndexOf || -1 == dllIndex)
                    {
                        return false;
                    }

                    return assembliesToLoad.Any(aName => aName == f.Substring(lastIndexOf + 1, dllIndex - lastIndexOf - 1));
                });

            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            toLoad.ForEach(path => dataAssembliesNames.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            return dataAssembliesNames.Count() == assembliesToLoad.Length;  // false = Not all assemblies were loaded into the  project!
        }
    }
}
