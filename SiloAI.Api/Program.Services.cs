using SiloAI.Agent.Chat;
using SiloAI.Agent.Rag;
using SiloAI.Api.Auth;
using SiloAI.Application.Api;
using SiloAI.Identity.Server;
using Microsoft.OpenApi.Models;
using Serilog;
using SiloAI.Api;

public static partial class Program
{
    public static IServiceCollection ConfigureAiApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSiloSerilog(configuration);

        services.AddAiDomainsServices(configuration);

        services.AddAiIdentityServerServices();

        services.AddAuthentication()
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationHandler.SchemeName,
                _ => { });

        services.AddScoped<ChatAgentService>();

        services.AddScoped<AiCostCalculator>();

        services.AddRagServices(configuration);

        services.AddApplicationAiApiServices();

        services.AddAuthorization();

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Admin JWT token — obtained from POST /admin/auth/login"
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}

