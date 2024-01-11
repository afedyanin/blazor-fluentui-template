using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTemplate.Components.Pages.Perspective;

public partial class PerspectiveGrid : IAsyncDisposable
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

    [Inject]
    private HttpClient Http { get; set; } = default;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var schema = await Http.GetFromJsonAsync<Dictionary<string, string>>(SchemaEndpoint);
            var data = await Http.GetFromJsonAsync<Dictionary<string, object>[]>(DataEndpoint);
            // var data = await Http.GetStringAsync(DataEndpoint);

            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorTemplate.Components/Pages/Perspective/PerspectiveGrid.razor.js");
            await _jsModule.InvokeVoidAsync("loadJson", schema, data, perspectiveViewer);
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
