using Apache.Arrow.Ipc;
using Dashboard.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Analysis;
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
        var df = ParquetToDataFrame(fileName);
        var batches = df.ToArrowRecordBatches();

        using var stream = new MemoryStream();
        ArrowStreamWriter? writer = null;

        foreach (var recordBatch in batches)
        {
            writer ??= new ArrowStreamWriter(stream, recordBatch.Schema);
            await writer.WriteRecordBatchAsync(recordBatch);
        }

        await writer!.WriteEndAsync();
        return File(stream, "application/apache.arrow");
    }

    private DataFrame ParquetToDataFrame(string fileName, int rowGroupIndex = 0)
    {
        var filepath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
        using var parquetReader = new ParquetFileReader(filepath);
        var df = parquetReader.ToDataFrame();
        parquetReader.Close();
        return df;
    }
}
