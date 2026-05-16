using FoodDelivery.DTOs.Cart;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartController (ICartService cartService)
    {
        _cartService = cartService;
    }
    
    [HttpGet]
    [HasPermission(PermissionConstants.Cart.View)]
    public async Task<IActionResult> GetCart([FromQuery] int page = 1, int pageSize = 10)
    {
        var userId = User.GetUserId();
        var result = await _cartService.GetCartAsync(userId, page, pageSize);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    
    [HttpPost]
    [HasPermission(PermissionConstants.Cart.AddItem)]
    public async Task<IActionResult> AddCartItem([FromBody] CartItemRequest cartItemRequest)
    {
        var userId = User.GetUserId();
        var result = await _cartService.AddCartItemAsync(userId,cartItemRequest);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return result.ErrorCode switch
        {
            "INVALID_QUANTITY" => BadRequest(result),
            "PRODUCT_NOT_FOUND" => NotFound(result),
            "PRODUCT_UNAVAILABLE" => BadRequest(result),
            _ => BadRequest(result)
        };
    }

    [HttpPatch("item/{productId}")]
    [HasPermission(PermissionConstants.Cart.UpdateItem)]
    public async Task<IActionResult> UpdateQuantity(Guid productId, [FromBody] UpdateQuantityRequest request)
    {
        var userId = User.GetUserId();
        var result = await _cartService.UpdateQuantityCartItemAsync(userId,productId,request.Quantity);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return result.ErrorCode switch
        {
            "INVALID_QUANTITY" => BadRequest(result),
            "PRODUCT_UNAVAILABLE" => BadRequest(result),
            "CART_NOT_FOUND" => NotFound(result),
            "PRODUCT_NOT_FOUND" => NotFound(result),
            "CART_ITEM_NOT_FOUND" => NotFound(result),
            _ => BadRequest(result)
        };
    }
    
    [HttpDelete("item/{productId}")]
    [HasPermission(PermissionConstants.Cart.RemoveItem)]
    public async Task<IActionResult> DeleteCartItem(Guid productId)
    {
        var userId = User.GetUserId();
        var result = await _cartService.DeleteCartItemAsync(userId,productId);
        return result.ToActionResult();
    }

    [HttpDelete("items")]
    [HasPermission(PermissionConstants.Cart.RemoveItem)]
    public async Task<IActionResult> DeleteCartItemRange([FromBody] DeleteCartItemsDto request)
    {
        var userId = User.GetUserId();
        var result = await _cartService.DeleteCartItemRangeAsync(userId,request.ProductIds);
        return result.ToActionResult();
    }
    [HttpDelete()]
    [HasPermission(PermissionConstants.Cart.ClearCart)]
    public async Task<IActionResult> DeleteCart()
    {
        var userId = User.GetUserId();
        var result = await _cartService.DeleteCartAsync(userId);
        return result.ToActionResult();
    }
}