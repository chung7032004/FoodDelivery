using FoodDelivery.Common;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Extensions;
public static class ResultExtensions
{
    public static IActionResult ToActionResult (this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result);
        }
        
        return new ObjectResult(result)
        {
            StatusCode = result.StatusCode
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result);

        return new ObjectResult(result)
        {
            StatusCode = result.StatusCode 
        }; 
    }
}