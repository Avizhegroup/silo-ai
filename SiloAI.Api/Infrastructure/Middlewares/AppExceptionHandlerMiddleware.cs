using global::SiloAI.Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;
using SiloAI.Shared;

public class AppExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AppExceptionHandlerMiddleware> logger;

    public AppExceptionHandlerMiddleware(RequestDelegate next
        , ILogger<AppExceptionHandlerMiddleware> logger)
    {
        _next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await LogRequest(context);

            await ConvertException(context, ex);
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        using StreamReader reader = new(context.Request.Body, Encoding.UTF8, true, 1024, true);

        string requestBody = await reader.ReadToEndAsync();

        logger.LogInformation($"{Environment.NewLine}Request Body:{Environment.NewLine} {requestBody}{Environment.NewLine}User Id: {context.User.GetUserId()}");

        context.Request.Body.Position = 0;
    }

    private Task ConvertException(HttpContext context, Exception exception)
    {
        logger.LogWarning(exception, exception.Message);

        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var result = string.Empty;

#if DEBUG
        Debugger.Break();
#endif

        switch (exception)
        {
            case SqlException sqlException:

                httpStatusCode = HttpStatusCode.ServiceUnavailable;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Messages = new[]
                    {
                        string.Format($"خطا در اجرای کوئری sql server با کد{sqlException.ErrorCode} به وجود آمده است ", sqlException.ErrorCode),
                        sqlException.Message
                    }
                });

                break;

            case SqliteException sqliteException:

                httpStatusCode = HttpStatusCode.BadGateway;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Messages = new[]
                    {
                        "خطایی در اجرای کوئری sqllite  به وجود امده است",
                        sqliteException.Message
                    },
                });

                break;

            case MethodNotFoundException methodNotFoundException:

                httpStatusCode = HttpStatusCode.NotFound;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Messages = new[]
                    {
                        methodNotFoundException.Message
                    },
                });

                break;

            case UserNotFoundException userNotFoundException:

                httpStatusCode = HttpStatusCode.Unauthorized;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Messages = new[]
                    {
                        userNotFoundException.Message
                    },
                });

                break;

            case TokenRequiredException tokenRequiredException:

                httpStatusCode = HttpStatusCode.Unauthorized;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Messages = new[]
                    {
                        tokenRequiredException.Message
                    },
                });

                break;

            case ProductNotFoundException productNotFoundException:

                httpStatusCode = HttpStatusCode.BadRequest;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Value = null,
                    Messages = productNotFoundException.Errors.ToArray()
                });

                break;

            case SqlServerConnectionStringException connectionStringException:

                httpStatusCode = HttpStatusCode.Ambiguous;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Value = null
                });

                break;

            case SiloValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Value = null,
                    Messages = validationException.ErrorMessages.ToArray(),
                });

                break;

            case MethodExecutionFailedException methodExecutionFailedException:

                httpStatusCode = HttpStatusCode.InternalServerError;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Value = null,
                    Messages = new[]
                    {
                       "در انجام عملیات مشکلی به وجود آمده است"
                    },
                });

                break;

            default:

                httpStatusCode = HttpStatusCode.InternalServerError;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Successful = false,
                    Value = null,
                    Messages = new[]
                    {
                        "در انجام عملیات مشکلی به وجود آمده است",
                        exception.Message
                    },
                });

                break;
        }

        context.Response.StatusCode = (int)httpStatusCode;

        return context.Response.WriteAsync(result);
    }
}
