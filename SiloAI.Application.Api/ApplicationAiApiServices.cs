using Microsoft.Extensions.DependencyInjection;

namespace SiloAI.Application.Api;

public static class ApplicationAiApiServices
{
    public static IServiceCollection AddApplicationAiApiServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAiApiServices).Assembly));

        return services;
    }
}
