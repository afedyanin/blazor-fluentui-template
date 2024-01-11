using BlazorTemplate.Client.Pages;
using BlazorTemplate.Server.Components;
using BlazorTemplate.Server.Infrastructure;
using BlazorTemplate.Shared.SampleData;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorTemplate.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7165") });


        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddFluentUIComponents();
        builder.Services.AddFluentUIDemoServices();
        builder.Services.AddScoped<DataSource>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseAuthorization();


        app.MapControllers();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Counter).Assembly);

        app.Run();
    }
}
