using FluentTemplate.Components.Infrastructure;

namespace FluentTemplate.Components.AccessControl;

public class UnauthorizedHandler : DelegatingHandler
{
    private readonly AppStateService _appStateService;
    public UnauthorizedHandler(AppStateService appStateService)
    {
        _appStateService = appStateService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _appStateService.NotifyUnauthorized();
        }

        return response;
    }
}
