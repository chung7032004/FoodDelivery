
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text.Json;
using FoodDelivery.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken = default)
    {
        if (exception is AggregateException agg)
        {
            
            exception = agg.Flatten().InnerException ?? agg;
            
        }
        // Phân loại Exception sang StatusCode và ErrorCode bằng Pattern Matching
        var (statusCode, errorCode, message) = exception switch
        {
            AppException appException => 
                (appException.StatusCode, appException.ErrorCode, appException.Message),
            SecurityException or UnauthorizedAccessException => 
                (StatusCodes.Status401Unauthorized, "UNAUTHORIZED", "Yêu cầu xác thực hoặc không có quyền."),
            
            KeyNotFoundException => 
                (StatusCodes.Status404NotFound, "NOT_FOUND", exception.Message),
            
            ArgumentException or JsonException or ValidationException => 
                (StatusCodes.Status400BadRequest, "INVALID_INPUT", exception.Message),
            
            OperationCanceledException
                when context.RequestAborted.IsCancellationRequested => 
                (499, "CLIENT_DISCONNECTED", "Client đã ngắt kết nối."),
            
            OperationCanceledException =>
                (StatusCodes.Status408RequestTimeout, "REQUEST_TIMEOUT", "Yêu cầu đã hết thời gian chờ."),

            DbUpdateException => 
                (StatusCodes.Status500InternalServerError, "DATABASE_ERROR", "Lỗi thao tác dữ liệu."),
            
            NotImplementedException => 
                (StatusCodes.Status501NotImplemented, "NOT_IMPLEMENTED", "Chức năng đang được phát triển."),
            
            _ => (StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR", "Đã có lỗi xảy ra, vui lòng thử lại sau.")
        };
        // Ghi log lỗi chi tiết
        if(statusCode >= 500)
        {
            var logEx = exception is DbUpdateException dbEx ? (dbEx.InnerException ?? dbEx) : exception;
            _logger.LogError(logEx,
                "Unhandled Exception [{ErrorCode}]: {Message}. TraceId: {TraceId}",
                errorCode, logEx.Message, context.TraceIdentifier);
        }
        else
        {
            _logger.LogWarning(
                "Handled Exception [{ErrorCode}]: {Message}. TraceId: {TraceId}",
                errorCode, message, context.TraceIdentifier);
        }
        // Trả về response lỗi chuẩn
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        var errorResponse = Result.Failure(errorCode, message, statusCode);
        await context.Response.WriteAsJsonAsync(errorResponse, JsonOptions, cancellationToken);
        return true; // Trả về true để kết thúc pipeline xử lý lỗi
    }
}