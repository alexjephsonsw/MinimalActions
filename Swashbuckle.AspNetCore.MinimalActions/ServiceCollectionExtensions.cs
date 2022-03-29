using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Swashbuckle.AspNetCore.MinimalActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMinimalActionsSwaggerGen(this IServiceCollection services, Action<SwaggerGenOptions>? action)
        {
            var swaggerGenOptions = new SwaggerGenOptions();
            action?.Invoke(swaggerGenOptions);

            var annotationsEnabled = swaggerGenOptions.OperationFilterDescriptors.FirstOrDefault(d => d.Type == typeof(AnnotationsOperationFilter)) is not null;

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                if (annotationsEnabled)
                {
                    options.OperationFilter<MinimalActionsSwaggerTagAttributeOperationFilter>();
                }
            });

            if (action is not null)
            {
                services.ConfigureSwaggerGen(action);
            }
            return services;
        }
    }
}