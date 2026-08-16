using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SiloAI.Identity.Server.Services;
using SiloAI.Identity.Server.Utilities;
using System.Net;
using System.Text.Json;

namespace SiloAI.Identity.Server;

public static class SiloAiIdentityServerServices
{
    private const string Issuer = "SiloAiIdentity";
    private const string Audience = "SiloAiAdmin";
    private const string SigningKey = "S1L0A1I2D3E4N5T6I7T8Y9K0E1Y2P3R4";

    public static IServiceCollection AddAiIdentityServerServices(this IServiceCollection services)
    {
        services.AddScoped<AiIdentityBusiness>();

        services.AddScoped<IAiJwtService, AiJwtService>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new()
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AiCryptoTools.GetSymmetricKey(SigningKey),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateIssuer = true,
                ValidIssuer = Issuer
            };

            options.Events = new()
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();

                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        Successful = false,
                        Message = "Unauthorized — valid admin token required."
                    }));
                }
            };
        });

        return services;
    }
}
