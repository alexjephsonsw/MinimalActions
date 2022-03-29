using Microsoft.Extensions.DependencyInjection;

namespace MinimalActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMinimalActions(this IServiceCollection services)
        {
            services.AddSingleton<IActionDelegateFactory, DefaultActionDelegateFactory>();
            services.AddSingleton<IAssemblyProvider, AssemblyProvider>();
            services.AddSingleton<IActionTypeProvider, ActionTypeProvider>();
            services.AddSingleton<IActionDescriptorProvider, DefaultActionDescriptorProvider>();
            services.AddSingleton<IActionDescriptorGenerator, DefaultActionDescriptorGenerator>();
            return services;
        }

        public static IServiceCollection AddActionDescriptorGenerator<T>(this IServiceCollection service) where T : class, IActionDescriptorGenerator
        {
            return service.AddSingleton<IActionDescriptorGenerator, T>();
        }
    }
}