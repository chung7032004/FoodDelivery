namespace FoodDelivery.Common;
public class Result
{
    public bool IsSuccess {get;}
    public string? ErrorCode {get;}
    public string? Message {get;}
    public int StatusCode {get;}
    protected Result(bool success, string? errorCode, string? message, int statusCode)
    {   
        IsSuccess = success;
        ErrorCode = errorCode;
        Message = message;
        StatusCode = statusCode;
    }
    public static Result Success()
    {
        return new Result( 
            success: true,
            errorCode: null,
            message: null,
            statusCode: 200
        );
    }

    public static Result Failure(string errorCode,string message, int statusCode = 400)
    {
        return new Result(
            success: false,
            errorCode: errorCode,
            message:message,
            statusCode: statusCode
        );
    }  
}

public class Result<T> : Result
{
    public T? Data {get;}
    private Result (bool success, T? data, string? errorCode, string? message, int statusCode)
        : base(success, errorCode, message, statusCode)
    {
        Data = data;
    }
    public static Result<T> Success(T data)
    {
        return new Result<T>(
        success: true,
        data: data,
        errorCode: null,
        message: null,
        statusCode: 200);
    } 
    public static new Result<T> Failure(string errorCode, string message, int statusCode = 400)
    {
        return new Result<T>(
        success: false,
        data: default,
        errorCode: errorCode,
        message: message,
        statusCode: statusCode
    );
    }
} 

public class AppException : Exception
{
    public string ErrorCode {get;}
    public int StatusCode{get;}
    public AppException(string errorCode, string message, int statusCode = 400)
        : base (message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}