using FluentTemplate.Components.AccessControl;
using FluentTemplate.Components.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentTemplate.BlazorApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddFluentUIComponents();

        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        builder.Services.AddControllers();


        builder.Services.AddScoped<AppStateService>();
        builder.Services.AddTransient<UnauthorizedHandler>();

        builder.Services.AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7279/");
        }).AddHttpMessageHandler<UnauthorizedHandler>();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(Components.Pages.Home).Assembly);

        app.MapControllers();

        app.Run();
    }
}
