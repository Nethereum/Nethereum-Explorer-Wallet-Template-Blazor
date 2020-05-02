using Microsoft.AspNetCore.Blazor.Hosting;
using System.Threading.Tasks;

namespace NethereumBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            Startup.ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }
    }
}
