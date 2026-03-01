using FluentTemplate.Components.Infrastructure;

namespace FluentTemplate.Components.AccessControl;

public class UnauthorizedHandler : DelegatingHandler
{
    private readonly AppStateService _appState;
    public UnauthorizedHandler(AppStateService appStateService)
    {
        _appState = appStateService;
        Console.WriteLine($"_appState.Id={_appState.Id}");
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _appState.NotifyUnauthorized();
        }

        return response;
    }
}
