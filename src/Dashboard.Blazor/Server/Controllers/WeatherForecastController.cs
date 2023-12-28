using Apache.Arrow;
using Apache.Arrow.Ipc;
using Dashboard.Blazor.Server.Helpers;
using Dashboard.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;
using ParquetSharp;

namespace Dashboard.Blazor.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 50).Select(index => new WeatherForecast
        {
            Date = DateTime.Today.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            Value = (decimal)Random.Shared.NextDouble() * 1000 * (Random.Shared.Next(0, 5) < 2 ? 1 : -1),
            IsCold = Random.Shared.Next(0, 5) < 2 ? true : false,
        })
        .ToArray();
    }

    [HttpGet("arrow/{fileName}")]
    public IActionResult GetArrow(string fileName)
    {
        var filepath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
        return File(System.IO.File.ReadAllBytes(filepath), "application/apache.arrow", Path.GetFileName(filepath));
    }

    [HttpGet("parquet/{fileName}")]
    public async Task<IActionResult> GetArrowBatch(string fileName)
    {
        var stream = new MemoryStream();
        ArrowStreamWriter? writer = null;

        foreach (var recordBatch in ArrowDataHelper.ParquetToArrow())
        {
            writer ??= new ArrowStreamWriter(stream, recordBatch.Schema);
            await writer.WriteRecordBatchAsync(recordBatch);
        }

        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(stream, "application/apache.arrow")
        {
            FileDownloadName = "file.arrow",
        };
    }

}
