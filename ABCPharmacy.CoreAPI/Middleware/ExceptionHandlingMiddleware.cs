using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace ABCPharmacy.CoreAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
            private readonly ILogger<ExceptionHandlingMiddleware> _logger;
            private readonly RequestDelegate _next;

            public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
            {
                _logger = logger;
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
                    if (ex.InnerException != null)
                    {
                        _logger.LogError("{ErrorType},{ErrorMessage}", ex.InnerException, ex.Message);
                    }
                    else
                    {
                        _logger.LogError("{ErrorType},{ErrorMessage}", ex.GetType(), ex.Message);
                    }
                    await ErrorHandler(context, ex);
                }
            }
            private Task ErrorHandler(HttpContext context, Exception ex)
            {
                ProblemDetails details = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "An unexpected Error Occured While Processing the Request",
                    Detail = "Internal Server Error Has Occured"
                };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(details));
            }
     }
    
}
