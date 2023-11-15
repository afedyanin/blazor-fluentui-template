using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Fast.Components.FluentUI;

namespace Dashboard.Blazor;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddFluentUIComponents(options =>
        {
            options.HostingModel = BlazorHostingModel.WebAssembly;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();

        var provider = new FileExtensionContentTypeProvider();
        provider.Mappings[".parquet"] = "application/apache.parquet";
        provider.Mappings[".arrow"] = "application/apache.arrow";
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = provider
        });

        app.UseRouting();


        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
