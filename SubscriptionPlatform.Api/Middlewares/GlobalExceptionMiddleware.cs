using System.Net;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Common.Exceptions;
using System.Text.Json;

namespace SubscriptionPlatform.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = GetStatusCode(exception);
            context.Response.StatusCode = statusCode;

            // RFC 7807 Standardı: ProblemDetails
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitle(exception),
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            // validation ise detay ekleme;
            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("errors", validationException.Errors);
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
        }

        private static int GetStatusCode(Exception exception) => exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        private static string GetTitle(Exception exception) => exception switch
        {
            ValidationException => "Validation Error",
            NotFoundException => "Not Found",
            _ => "Server Error"
        };
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}