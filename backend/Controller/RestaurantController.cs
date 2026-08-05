using FoodDelivery.DTOs.Restaurant;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;
namespace FoodDelivery.Controllers;
[ApiController]
[Route("api/restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet("profile")]
    [HasPermission(PermissionConstants.Restaurants.View)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.GetUserId();
        var result = await _restaurantService.GetMyRestaurantAsync(userId);
        if (!result.IsSuccess)
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                _ => BadRequest(result)
            };
        return Ok(result);
    }

    [HttpGet("status")]
    [HasPermission(PermissionConstants.Restaurants.ManageSettings)]
    public async Task<IActionResult> GetStatus()
    {
        var userId = User.GetUserId();
        var result = await _restaurantService.GetStatusRestaurantAsync(userId);
        if (!result.IsSuccess)
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                _ => BadRequest(result)
            };
        return Ok(result);
    }

    [HttpPut("open")]
    [HasPermission(PermissionConstants.Restaurants.ManageSettings)]
    public async Task<IActionResult> Open()
    {
        var userId = User.GetUserId();
        var result = await _restaurantService.OpenRestaurantAsync(userId);
        if (!result.IsSuccess)
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                "RESTAURANT_ALREADY_OPEN" => BadRequest(result),
                _ => BadRequest(result)
            };
        return Ok(result);
    }

    [HttpPut("close")]
    [HasPermission(PermissionConstants.Restaurants.ManageSettings)]
    public async Task<IActionResult> Close([FromBody] CloseRestaurantRequest request)
    {
        var userId = User.GetUserId();
        var result = await _restaurantService.CloseRestaurantAsync(userId, request);
        if (!result.IsSuccess)
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                "RESTAURANT_ALREADY_CLOSE" => BadRequest(result),
                _ => BadRequest(result)
            };
        return Ok(result);
    }

    [HttpPut("profile")]
    [HasPermission(PermissionConstants.Restaurants.UpdateProfile)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateRestaurantProfileRequest request)
    {
        var userId = User.GetUserId();
        var result = await _restaurantService.UpdateRestaurantAsync(userId, request);
        if (!result.IsSuccess)
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                _ => BadRequest(result)
            };
        return Ok(result);
    }
}