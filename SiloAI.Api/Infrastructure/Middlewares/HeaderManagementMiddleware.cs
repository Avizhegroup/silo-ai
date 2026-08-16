public class HeaderManagementMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HeaderManagementMiddleware> logger;

    public HeaderManagementMiddleware(RequestDelegate next
        , ILogger<HeaderManagementMiddleware> logger)
    {
        _next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

        context.Response.Headers.Remove("Server");

        await _next(context);
    }
}
