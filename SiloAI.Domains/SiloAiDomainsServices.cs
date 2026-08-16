using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SiloAI.Domains;
public static class SiloAiDomainsServices
{
    public static IServiceCollection AddAiDomainsServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AiApiContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AiDb")));

        return services;
    }
}
