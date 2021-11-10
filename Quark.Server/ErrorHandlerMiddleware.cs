using Quark.Shared;
using Quark.Shared.Wrapper;
using System.Net;
using System.Text.Json;

internal class ErrorHandlerMiddleware
{
    private RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = await Result<string>.FailAsync(e.Message);
            switch (e)
            {
                case ApiException ex:
                    //Custom Application Error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException ex:
                    //Not Found Error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    //Unhandled Error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}