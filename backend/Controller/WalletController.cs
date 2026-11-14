using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controller;

[ApiController]
[Route("api/wallet")]
[Authorize]
public sealed class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    /// <summary>Gets the authenticated user's wallet balance.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
    {
        var result = await _walletService.GetBalanceAsync(User.GetUserId(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets the authenticated user's paged wallet transaction history.</summary>
    [HttpGet("transactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _walletService.GetHistoryAsync(
            User.GetUserId(),
            page,
            pageSize,
            cancellationToken);
        return Ok(result);
    }
}
