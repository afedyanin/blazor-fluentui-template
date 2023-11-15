using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Blazor.Server.Controllers;

public class TripDataController : ControllerBase
{
    [Route("/tripdata")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var bytes = GetBytes("weather.json.arrow");
            var segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await webSocket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);


            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                return;
            }
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }


    private static byte[] GetBytes(string fileName)
    {
        var filepath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
        return System.IO.File.ReadAllBytes(filepath);
    }
}
