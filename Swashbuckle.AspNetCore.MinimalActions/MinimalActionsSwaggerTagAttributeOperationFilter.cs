using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Reflection;

namespace Swashbuckle.AspNetCore.MinimalActions
{
    internal class MinimalActionsSwaggerTagAttributeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ApplySwaggerTagAttribute(operation, context.MethodInfo);
        }

        private static void ApplySwaggerTagAttribute(OpenApiOperation operation, MethodInfo? methodInfo)
        {
            var swaggerTagAttribute = methodInfo?.DeclaringType?.GetCustomAttribute<SwaggerTagAttribute>();
            if (swaggerTagAttribute != null && swaggerTagAttribute.Description != null)
            {
                operation.Tags = new List<OpenApiTag> { new OpenApiTag { Name = swaggerTagAttribute.Description } };
            }
        }
    }
}