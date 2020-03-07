using System.IO;
using System.Threading.Tasks;
using Microkernel.Contract;
using Microkernel.Core;
using Microsoft.Extensions.Configuration;

namespace Microkernel
{
    class Program
    {
        public static PluginRegistry registry { get; set; }

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();
            var plugins = configuration.GetSection("plugins").Get<PluginEntry[]>();
            var pluginsLocation = configuration.GetSection("pluginsLocation").Get<string>();

            registry = new PluginRegistry();
            registry.LoadRegistry(pluginsLocation, plugins);

            long i = 0;
            while (true)
            {
                var result = ProcessItem(i++);
                ProcessResult(result);

                await Task.Delay(1000);
            }
        }



        private static long ProcessItem(long value)
        {
            var plugins = registry.GetPlugins<IProcessItemPlugin>("ProcessItem");

            foreach (var plugin in plugins)
            {
                value = plugin.Process(value);
            }

            return value;
        }

        private static void ProcessResult(long result)
        {
            var plugins = registry.GetPlugins<IProcessResultPlugin>("ProcessResult");

            foreach (var plugin in plugins)
            {
                plugin.ProcessResult(result);
            }
        }
    }
}
