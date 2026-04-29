using FoodDelivery.DTOs;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    public AddressController (IAddressService addressService)
    {
        _addressService = addressService;
    }
    [HttpGet]
    [HasPermission(PermissionConstants.Address.View)]
    public async Task<IActionResult> GetMyAddress()
    {
        var userId = User.GetUserId();
        var result = await _addressService.GetMyAddresses(userId);
        return Ok(result);
    }
    [HttpGet("{id}")]
    [HasPermission(PermissionConstants.Address.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var result = await _addressService.GetAddressById(id, userId);
        if (!result.IsSuccess)
        {
            return BadRequest(result); 
        }
        return Ok(result);
    }
    [HttpPost]
    [HasPermission(PermissionConstants.Address.Create)]
    public async Task<IActionResult> Create(AddAddressRequest request)
    {
        var userId = User.GetUserId();
        var result = await _addressService.AddAddress(userId,request);
        if (result.IsSuccess && result.Data != null)
        {
            return CreatedAtAction(nameof(GetById),new {id = result.Data.Id},result);
        }
        return BadRequest(result);
    }
    [HttpPut("{id}")]
    [HasPermission(PermissionConstants.Address.Edit)]
    public async Task<IActionResult> Update(Guid id , AddressRequest addressRequest)
    {
        var userId = User.GetUserId();
        var result = await _addressService.UpdateAddress(userId,id, addressRequest);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }
    [HttpDelete("{id}")]
    [HasPermission(PermissionConstants.Address.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _addressService.DeleteAddress(id);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }
    [HttpPatch("{id}/default")]
    [HasPermission(PermissionConstants.Address.SetDefault)]
    public async Task<IActionResult> SetDefault(Guid id)
    {
        var userId = User.GetUserId();
        var result = await _addressService.SetDefault(userId,id);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}