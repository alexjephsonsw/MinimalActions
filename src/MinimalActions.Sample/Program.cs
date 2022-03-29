using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.MinimalActions;
using System;
using System.Linq;

namespace MinimalActions.Sample
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMinimalActionsSwaggerGen(options =>
            {
                options.IncludeXmlComments("./MinimalActions.Sample.Xml");
                options.EnableAnnotations();
                options.SupportNonNullableReferenceTypes();
            });

            builder.Services.AddMinimalActions();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapGet("/weatherforecast", () =>
            {
                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };

                var forecast = Enumerable.Range(1, 5).Select(index =>
                   new WeatherForecast
                   (
                       DateTime.Now.AddDays(index),
                       Random.Shared.Next(-20, 55),
                       summaries[Random.Shared.Next(summaries.Length)]
                   ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithTags("Weather Forecast");

            app.MapGet("/custom-handler/{id}", StaticAction.CustomHandler);

            app.UseMinimalActions();

            app.Run();
        }

        internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }
    }
}