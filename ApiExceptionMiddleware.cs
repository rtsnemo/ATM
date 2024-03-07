using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace ATM
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                Console.WriteLine("It works!");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch(ex)
            {
                case InvalidOperationException invalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(invalidOperationException.Message);
                    break;
                case ArgumentException argumentException:
                    code = HttpStatusCode.BadGateway;
                    result = JsonSerializer.Serialize(argumentException.Message);
                    break;
                default:
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
