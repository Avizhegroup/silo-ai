using Newtonsoft.Json;
using SiloAI.Application.Shared;
using System.Text;
using System.Text.RegularExpressions;

public class InvalidContentCheckMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<InvalidContentCheckMiddleware> logger;

    public InvalidContentCheckMiddleware(RequestDelegate next
        , ILogger<InvalidContentCheckMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestContent = context.Request.QueryString.Value ?? "";

        if (context.Request.HasFormContentType)
        {
            foreach (var formValue in context.Request.Form)
            {
                requestContent += formValue;
            }
        }

        if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);

            string? body = await reader.ReadToEndAsync();

            requestContent += body;

            context.Request.Body.Position = 0;
        }

        var path = context.Request.Path.Value;

        if (path?.StartsWith( "/RfidCore/v2/ChatSessions", StringComparison.OrdinalIgnoreCase) == true || path?.StartsWith( "/api/ai/chat/send", StringComparison.OrdinalIgnoreCase) == true)
        {
            await next(context);
            return;
        }

        if (ContainsSqlInjection(requestContent))
        {
            logger.LogWarning("Possible SQL injection attempt detected: {RequestContent}", requestContent);

            var result = JsonConvert.SerializeObject(new ApiResponse()
            {
                Successful = false,
                Messages = ["Invalid request detected."],
                Value = 4
            });

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync(result);

            return;
        }

        await next(context);
    }

    private bool ContainsSqlInjection(string input)
    {
        if (input.HasNoValue())
        {
            return false;
        }

        string[] sqlInjectionPatterns =
        {
            @"(\%27)|(\')|(\-\-)|(\%23)|(#)",
            @"((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))",
            @"\b(select|update|delete|insert|exec|union|drop|alter|declare|cast|convert)\b"
        };

        foreach (var pattern in sqlInjectionPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
