using System;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace uni.learn.api.Middleware;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;


    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
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
        catch (Exception e)
        {
            _logger.LogError(e, "Error: {Message}", e.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            };

            switch (e)
            {
                case DbUpdateException dbUpdateEx:
                    if (dbUpdateEx.InnerException?.Message?.Contains("duplicate") ?? false)
                    {
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        errorResponse.Message = "Ya existe un registro con estos datos.";
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.Code = "DATABASE_ERROR";
                        errorResponse.Message = "Error al actualizar la base de datos.";
                    }
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Code = "NOT_FOUND";
                    errorResponse.Message = "El recurso solicitado no fue encontrado.";
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Code = "UNAUTHORIZED";
                    errorResponse.Message = "No está autorizado para realizar esta acción.";
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Code = "INVALID_ARGUMENT";
                    errorResponse.Message = argEx.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Code = "INTERNAL_SERVER_ERROR";
                    errorResponse.Message = "Ha ocurrido un error interno del servidor.";
                    break;
            }



            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
            await response.WriteAsync(json);

        }


    }

     public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public string Timestamp { get; set; }
        public DeveloperMessage DeveloperMessage { get; set; }
    }

    public class DeveloperMessage
    {
        public string Exception { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

}
