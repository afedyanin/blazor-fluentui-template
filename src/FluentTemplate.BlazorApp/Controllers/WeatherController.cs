using FluentTemplate.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FluentTemplate.BlazorApp.Controllers;

[Route("api/weather")]
[ApiController]

public class WeatherController : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetWeather()
    {
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        });

        return Ok(forecasts);
    }
}
