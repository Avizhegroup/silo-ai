using Microsoft.AspNetCore.Builder;

public static class SiloInfrastructureSharedMiddlewares
{
    public static void UseInfrastructureSharedMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<InvalidContentCheckMiddleware>();

        app.UseMiddleware<HeaderManagementMiddleware>();

        app.UseMiddleware<RequestLoggingMiddleware>();

        app.UseMiddleware<AppExceptionHandlerMiddleware>();
    }
}
