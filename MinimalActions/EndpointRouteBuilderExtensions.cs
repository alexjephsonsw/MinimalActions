using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalActions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void UseMinimalActions(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var actionDelegateFactory = endpointRouteBuilder.ServiceProvider.GetRequiredService<IActionDelegateFactory>();
            var actionDescriptorProvider = endpointRouteBuilder.ServiceProvider.GetRequiredService<IActionDescriptorProvider>();

            foreach (var descriptor in actionDescriptorProvider.Descriptors)
            {
                if (descriptor.RoutePattern is null)
                {
                    continue;
                }

                var actionDelegate = actionDelegateFactory.Create(descriptor, endpointRouteBuilder.ServiceProvider);

                endpointRouteBuilder.MapMethods(descriptor.RoutePattern, descriptor.HttpMethods, actionDelegate)
                    .WithDisplayName(descriptor.DisplayName)
                    .WithTags(descriptor.ActionType.Name)
                    .WithMetadata(descriptor.MethodAttributes);
            }
        }
    }
}