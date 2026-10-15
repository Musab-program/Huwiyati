namespace Huwiyati.API.Middleware;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Global exception handling middleware that catches unhandled exceptions and writes RFC 9457 compliant ProblemDetails JSON responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="env">The host environment information.</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Invokes the middleware to handle the request and catch any unhandled exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while processing request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // RFC 9457 compliant content type
        context.Response.ContentType = "application/problem+json";

        var (statusCode, title, defaultDetail) = exception switch
        {
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Resource Not Found",
                "The requested resource was not found."),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                "You are not authorized to access this resource."),
            ArgumentException argEx => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                argEx.Message),
            InvalidOperationException invEx => (
                StatusCodes.Status400BadRequest,
                "Invalid Operation",
                invEx.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected internal error occurred on the server.")
        };

        context.Response.StatusCode = statusCode;

        var detail = _env.IsDevelopment() ? exception.Message : defaultDetail;

        // RFC 9457 Problem Details object
        var problemDetails = new ProblemDetails
        {
            Type = GetRfcTypeUri(statusCode),
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        // RFC 9457 standard extension members
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        // Project compatibility field
        problemDetails.Extensions["isSuccess"] = false;

        // In Development environment, expose exception diagnostics
        if (_env.IsDevelopment())
        {
            problemDetails.Extensions["exception"] = new
            {
                type = exception.GetType().FullName,
                message = exception.Message,
                stackTrace = exception.StackTrace,
                innerException = exception.InnerException?.Message
            };
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _env.IsDevelopment()
        };

        var json = JsonSerializer.Serialize(problemDetails, jsonOptions);
        await context.Response.WriteAsync(json);
    }

    private static string GetRfcTypeUri(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
        StatusCodes.Status401Unauthorized => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2",
        StatusCodes.Status403Forbidden => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4",
        StatusCodes.Status404NotFound => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
        StatusCodes.Status500InternalServerError => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
        _ => $"https://httpstatuses.io/{statusCode}"
    };
}
