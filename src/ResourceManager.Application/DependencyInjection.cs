using Microsoft.Extensions.DependencyInjection;
using ResourceManager.Application.Common;
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

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        
        return services;
    }
}