using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenAPI.MVC.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[SwaggerTag("Create and read WeatherForecast")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Cities = new[]
{
        "San Fransico", "New York", "London", "Paris", "Berlin", "Tokyo"
    };

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Test 2
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWeatherForecast")]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns the newly created item")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Oops!")]
    public IEnumerable<WeatherForecast> Get(
         [FromQuery, SwaggerParameter("Some Id", Required = true)] int id
       )
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            City = Summaries[Random.Shared.Next(Cities.Length)],
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Creates a new weather forecast
    /// </summary>
    /// <param name="name" example="New Your">The city name</param>
    [HttpPost(Name = "CreateWeatherForecast")]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns the newly created item")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Oops!")]
    public WeatherForecast Create(
         [FromBody, SwaggerRequestBody("The payload", Required = true)] string name
        )
    {
        return new WeatherForecast
        {
            City = name,
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };
    }
}

