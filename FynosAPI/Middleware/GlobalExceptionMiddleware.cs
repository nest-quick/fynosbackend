using FynosAPI.Dtos;
using System.Net;
using System.Text.Json;

namespace FynosAPI.Middleware
{
    // This middleware catches any unhandled exception that occurs
    // anywhere later in the request pipeline (controllers, services,
    // Entity Framework, etc.).
    public class GlobalExceptionMiddleware
    {
        // RequestDelegate represents the NEXT middleware in the pipeline.
        // Calling _next(context) passes the request to the next component.
        private readonly RequestDelegate _next;

        // ILogger allows us to write structured logs instead of using Console.WriteLine.
        // Logs can later be sent to files, databases, or cloud services.
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        // IHostEnvironment tells us whether the application is running
        // in Development, Staging, or Production.
        // We'll use this to decide how much error information to return.
        private readonly IHostEnvironment _environment;

        // ASP.NET Core's dependency injection automatically provides
        // these dependencies when the middleware is created.
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        // Every HTTP request passes through this method.
        // Think of it as the entry point of the middleware.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware.
                // Eventually this reaches the controller.

                // If nothing throws an exception,
                // execution simply returns here and the response
                // continues back to the client.
                await _next(context);
            }
            catch (Exception exception)
            {
                // If ANY unhandled exception occurs later in the pipeline,
                // execution jumps here instead of crashing the application.

                await HandleExceptionAsync(context, exception);
            }
        }

        // This method creates a consistent error response
        // whenever an exception occurs.
        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            // Log the exception.

            // Notice the placeholders:
            // {Method}, {Path}, {TraceId}

            // This is structured logging.
            // Instead of building one long string,
            // each value is stored separately, making it much easier
            // to search and analyze in logging systems.

            _logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            // Set the HTTP status code.
            // 500 means "Internal Server Error".

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            // Tell the client that we're returning JSON.
            context.Response.ContentType = "application/json";

            // Build the JSON object that the frontend receives.

            // We purposely do NOT expose the full exception in production.

            var response = new ApiErrorResponseDto
            {
                StatusCode = context.Response.StatusCode,

                // Generic message shown to users.
                Message = "An unexpected error occurred.",

                // Useful for matching frontend errors
                // with backend logs.
                TraceId = context.TraceIdentifier,

                // During development we include the exception message.
                // In production this becomes null for security.
                Details = _environment.IsDevelopment()
                    ? exception.Message
                    : null
            };

            // Convert the anonymous object into JSON.
            var json = JsonSerializer.Serialize(response);

            // Write the JSON into the HTTP response body.
            await context.Response.WriteAsync(json);
        }
    }
}
