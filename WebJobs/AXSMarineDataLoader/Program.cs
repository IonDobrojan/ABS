using Microsoft.Azure;
using Microsoft.Azure.WebJobs;

namespace AXSMarineDataLoader
{
    internal class Program
    {
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var host = new JobHost(config);
            host.Call(typeof(Functions).GetMethod("LoadData"));
        }
    }
}
