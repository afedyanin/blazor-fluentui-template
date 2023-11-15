namespace Dashboard.Blazor.Shared;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public decimal Value { get; set; }

    public bool IsCold { get; set; }
}
