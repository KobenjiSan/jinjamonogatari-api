using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // get status code based on exception thrown
            var statusCode = ex switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            // format output
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = "An unexpected error occurred",
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            // prepare return
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
