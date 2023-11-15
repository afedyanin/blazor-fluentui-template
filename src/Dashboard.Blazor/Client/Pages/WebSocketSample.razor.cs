using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Blazor.Client.Pages;

public partial class WebSocketSample : IAsyncDisposable
{
    private IJSObjectReference? _jsModule;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/WebSocketSample.razor.js");
            await _jsModule.InvokeVoidAsync("initElements");
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Client disconnected.
        }
    }


}
