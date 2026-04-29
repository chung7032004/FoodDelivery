using FoodDelivery.DTOs.Restaurant;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;
namespace FoodDelivery.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _restaurantService.GetMyRestaurantAsync();
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                _=>BadRequest(result)
            };
        }
        return Ok(result);
    }
    // xem trạng thái quán
    [HttpGet("status")]
    public async Task<ActionResult> GetStatus()
    {
        var result = await _restaurantService.GetStatusRestaurantAsync();
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                _=>BadRequest(result)
            };
        }
        return Ok(result);
    }
    [HttpPut("open")]
    [HasPermission(PermissionConstants.Restaurants.ManageSettings)]
    public async Task<IActionResult> Open()
    {
        var result = await _restaurantService.OpenRestaurantAsync();
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                "RESTAURANT_ALREADY_OPEN" => BadRequest(result),
                _=>BadRequest(result)
            };
        }
        return Ok(result);
    }
    [HttpPut("Close")]
    [HasPermission(PermissionConstants.Restaurants.ManageSettings)]
    public async Task<IActionResult> Close([FromBody] CloseRestaurantRequest request)
    {
        var result = await _restaurantService.CloseRestaurantAsync(request);
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                "RESTAURANT_ALREADY_ClOSE" => BadRequest(result),
                _=>BadRequest(result)
            };
        }
        return Ok(result);
    }
    [HttpPut("profile")]
    [HasPermission(PermissionConstants.Restaurants.UpdateProfile)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateRestaurantProfileRequest request)
    {
        var result = await _restaurantService.UpdateRestaurantAsync(request);
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "RESTAURANT_NOT_FOUND" => NotFound(result),
                _=>BadRequest(result)
            };
        }
        return Ok(result);
    }
}