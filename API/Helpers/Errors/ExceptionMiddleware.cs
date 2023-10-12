using System.Net;
using System.Text.Json;

namespace API.Helpers.Errors;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        } catch (Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = _env.IsDevelopment()
                ? new ApiExcepcion(statusCode, ex.Message, ex.StackTrace.ToString())
                : new ApiExcepcion(statusCode);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
