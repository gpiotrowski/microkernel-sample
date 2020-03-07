using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microkernel.Contract;

namespace Microkernel.Core
{
    public class PluginRegistry
    {
        private Dictionary<string, IEnumerable<PluginEntry>> registry = new Dictionary<string, IEnumerable<PluginEntry>>();

        public void LoadRegistry(string pluginsLocation, IEnumerable<PluginEntry> registeredPlugins)
        {
            var groupedPluginEntries = registeredPlugins.GroupBy(x => x.OperationId);
            foreach (var groupedPluginEntry in groupedPluginEntries)
            {
                registry.Add(groupedPluginEntry.Key, groupedPluginEntry.AsEnumerable());
            }

            LoadAllAssembliesFromLocation(pluginsLocation);
        }

        private void LoadAllAssembliesFromLocation(string location)
        {
            var files = Directory.GetFiles(location);
            foreach (var file in files)
            {
                LoadAssembly(file);
            }
        }

        private void LoadAssembly(string path)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        }

        public IEnumerable<T> GetPlugins<T>(string operationId) where T : IPlugin
        {
            if (registry.TryGetValue(operationId, out var pluginEntries))
            {
                var plugins = pluginEntries.Select(CreatePluginInstance<T>);

                return plugins;
            }

            return Array.Empty<T>();
        }

        private T CreatePluginInstance<T>(PluginEntry pluginEntry) where T : IPlugin
        {
            var type = Type.GetType(pluginEntry.Type);
            if (type == null)
            {
                throw new Exception("Error - plugin wasn't loaded correctly during initialization.");
            }

            var pluginInstance = (T)Activator.CreateInstance(type);
            if (pluginInstance == null)
            {
                throw new Exception("Error - couldn't create plugin instance.");
            }

            return pluginInstance;
        }
    }

    public class PluginEntry
    {
        public string OperationId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }
}
