using Microsoft.Extensions.DependencyInjection;
using ResourceManager.Application.Users;

namespace ResourceManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        }); 
        
        return services;
    }
}