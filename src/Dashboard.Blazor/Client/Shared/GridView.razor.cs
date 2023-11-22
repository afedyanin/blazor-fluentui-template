using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Blazor.Client.Shared;

public partial class GridView : IAsyncDisposable
{
    private IJSObjectReference? _jsModule;

    private ElementReference perspectiveViewer;

    [Parameter]
    public string TableName { get; set; } = "Table";

    [Parameter]
    public string Height { get; set; } = "800px";

    [Parameter]
    public bool UseWebSocket { get; set; }

    [Parameter]
    public string SchemaEndpoint { get; set; } = string.Empty;

    [Parameter]
    public string DataEndpoint { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/GridView.razor.js");

            if (UseWebSocket)
            {
                await _jsModule.InvokeVoidAsync("fetchWebSocket", DataEndpoint, perspectiveViewer);
            }
            else
            {
                await _jsModule.InvokeVoidAsync("fetchArrow", DataEndpoint, perspectiveViewer);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null)
            {
                // await _jsModule.InvokeVoidAsync("dispose");
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Client disconnected.
        }
    }
}
