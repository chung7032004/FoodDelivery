namespace FoodDelivery.Common;
public static class HttpErrors
{
    public static Result NotFound(string code, string message) 
        => Result.Failure(code, message, 404);
    
    public static Result<T> NotFound<T>(string code, string message) 
        => Result<T>.Failure(code, message, 404);

    public static Result BadRequest(string code, string message) 
        => Result.Failure(code, message, 400);

    public static Result<T> BadRequest<T>(string code, string message) 
        => Result<T>.Failure(code, message, 400);

    public static Result Conflict(string code, string message) 
        => Result.Failure(code, message, 409);

    public static Result<T> Conflict<T>(string code, string message) 
        => Result<T>.Failure(code, message, 409);

    public static Result Forbidden(string code, string message)
        => Result.Failure(code, message, 403);
    public static Result<T> Forbidden<T>(string code, string message)
        => Result<T>.Failure(code, message, 403);
    public static Result ServiceUnavailable(string code, string message)
        => Result.Failure(code, message, 503);
    public static Result<T> ServiceUnavailable<T>(string code, string message)
        => Result<T>.Failure(code, message, 503);
    public static Result InternalServerError(string code, string message)        => Result.Failure(code, message, 500);
    public static Result<T> InternalServerError<T>(string code, string message)        => Result<T>.Failure(code, message, 500);
}