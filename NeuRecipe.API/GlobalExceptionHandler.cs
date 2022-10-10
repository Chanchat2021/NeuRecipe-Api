using NeuRecipe.Application.Exceptions;
using System.Net;
using System.Text.Json;
namespace NeuRecipe.API
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> logger;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                switch (error)
                {
                    case RecordNotFoundException ex:
                        logger.LogError("The information passed is not valid. More details : ", ex);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException ex:
                        logger.LogError("Cannot fetch the details from the given ID. More details :", ex);
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ArgumentNullException ex:
                        logger.LogError("Argument passed cannot be null. More details :", ex);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ArgumentOutOfRangeException ex:
                        logger.LogError("Argument passed is out of range. More details :", ex);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UnauthorizedAccessException ex:
                        logger.LogError("Not authorized for this request. More details :", ex);
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case DuplicateException ex:
                        logger.LogError("Already Present. More details", ex);
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case FormatException ex:
                        logger.LogError("The information passed is not valid. More details : ", ex);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.LogError($"Something went wrong", error);
                        break;
                }
                var result = JsonSerializer.Serialize(new { Error_Message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
