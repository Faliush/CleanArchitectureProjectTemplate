using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Web.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await _next(context);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			ProblemDetails problem = new()
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Type = "Server error",
				Title = "Server error",
				Detail = "An internal server error has occurred"
			};

			string json = JsonSerializer.Serialize(problem);

			await context.Response.WriteAsync(json);

			context.Response.ContentType = "application/json";
		}
    }
}
