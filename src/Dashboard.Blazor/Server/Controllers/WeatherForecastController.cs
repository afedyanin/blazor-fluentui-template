using Dashboard.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;

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

}
