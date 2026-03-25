using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FluentTemplate.Components.Pages;

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
#pragma warning disable CA1063 // Dispose methods should call SuppressFinalize

public partial class Home : IDisposable
{
    [Inject]
    private ILogger<Home> Logger { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {/*
        Logger.LogInformation($"OnInitializedAsync Called. ThreadId={Environment.CurrentManagedThreadId}");

        _ = Task.Run(async () =>
        {
            var guid = Guid.NewGuid();
            Logger.LogInformation($"Starting Task with Id={guid}");
            while (true)
            {
                Logger.LogInformation($"Task is running: Time={DateTime.UtcNow:O} Guid={guid}");
                await Task.Delay(3000);
            }
        });
        */
        return base.OnInitializedAsync();
    }

    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Dispose methods should call SuppressFinalize
    {
        // Logger.LogInformation("Disposing...");
    }
}
