using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenAPI.MVC.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
// https://github.com/domaindrivendev/Swashbuckle.AspNetCore

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "OPEN API",
        Description = "An OpenAPI ASP.NET Core Web API example",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    //Enrich OpenAPI metadata - requires the package Swashbuckle.AspNetCore.Annotations
    options.EnableAnnotations();

    // Alternative to the above way of annotating
    // Adds XMl comments to swagger UI
    // To generate the XML add <GenerateDocumentationFile>true</GenerateDocumentationFile> in the <PropertyGroup> to the csproj file
    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // https://localhost:<port>/swagger/v1/swagger.json
    // https://localhost:<port>/swagger/v1/swagger.yaml

    app.UseSwagger(options =>
    {
        options.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<OpenApiServer>
            {
                // You can add as many OpenApiServer instances as you want by creating them like below
                new OpenApiServer
                {
                    // You can set the Url from the default http request data or by hard coding it
                    // Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Url = "https://api.my-weather-app.org/",
                    Description = "Open Weather Map"
                }
            };
        });
    });   

    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        // Enables customization of the swagger ui - Remember to add app.UseStaticFiles()
        options.InjectStylesheet("/swagger-ui/custom.css");
        options.InjectJavascript("/swagger-ui/custom.js");

        //options.SwaggerEndpoint("/api-docs/v1/swagger.json", "api-docs API");
        //options.RoutePrefix = "api-docs"; //Changes path to the UI - default is "/swagger"

    });
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure the wwwroot folder exists

app.UseAuthorization();

app.MapControllers();

app.Run();

