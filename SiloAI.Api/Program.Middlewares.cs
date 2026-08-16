public static partial class Program
{
    public static WebApplication ConfigureAiApi(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseInfrastructureSharedMiddlewares();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
