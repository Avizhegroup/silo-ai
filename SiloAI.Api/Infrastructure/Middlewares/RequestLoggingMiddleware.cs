using SiloAI.Application.Shared;
using System.Text;
using SiloAI.Shared;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.Value.ToLower().Contains("account"))
        {
            context.Request.EnableBuffering();

            using StreamReader reader = new(context.Request.Body, Encoding.UTF8, true, 1024, true);

            string requestBody = await reader.ReadToEndAsync();

            if (requestBody.HasValue())
            {
                _logger.LogInformation($"{Environment.NewLine}Path:{Environment.NewLine} {context.Request.Path.Value}{Environment.NewLine}Request Body:{Environment.NewLine} {requestBody}{Environment.NewLine}User Id: {context.User.GetUserId()}");

                context.Request.Body.Position = 0;
            }
        }

        await _next(context);
    }

}
