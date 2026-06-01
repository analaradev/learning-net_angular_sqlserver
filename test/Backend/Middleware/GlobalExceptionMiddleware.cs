using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError(
                exception,
                "Ocurrio un error inesperado al procesar {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Error interno del servidor",
                Detail = "Ocurrio un error inesperado.",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
