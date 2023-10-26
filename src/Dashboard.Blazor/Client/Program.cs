using Dashboard.Blazor.Client.SampleData;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;

namespace Dashboard.Blazor.Client;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddFluentUIComponents(options =>
        {
            options.HostingModel = BlazorHostingModel.WebAssembly;
        });

        builder.Services.AddScoped<DataSource>();

        await builder.Build().RunAsync();
    }
}
