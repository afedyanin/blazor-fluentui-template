using System.Net.WebSockets;
using Apache.Arrow.Ipc;
using Dashboard.Blazor.Server.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Blazor.Server.Controllers;

public class TripDataController : ControllerBase
{
    [Route("/tripdata")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            for (int batch = 0; batch <= 0; batch++)
            {
                var bytes = await GetBytes(batch);
                var segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await webSocket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task<byte[]> GetBytes(int batchNum)
    {
        var stream = new MemoryStream();
        ArrowStreamWriter? writer = null;

        foreach (var recordBatch in ArrowDataHelper.ParquetToArrow(batchNum))
        {
            writer ??= new ArrowStreamWriter(stream, recordBatch.Schema);
            await writer.WriteRecordBatchAsync(recordBatch);
        }

        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        return stream.ToArray();
    }
}
