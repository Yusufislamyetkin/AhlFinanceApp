using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Application.DTOs;
using AhlApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Yeni bir gider oluşturur.
    /// </summary>
    [HttpPost("expense")]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto)
    {
        var userId = HttpContext.GetUserId();
        var result = await _transactionService.CreateExpenseAsync(userId, dto);
        if (!result.Success)
            return BadRequest(new { Message = result.ErrorMessage });

        return Ok(new { TransactionId = result.Data });
    }

    /// <summary>
    /// Yeni bir para yatırma işlemi yapar.
    /// </summary>
    [HttpPost("deposit")]
    public async Task<IActionResult> DepositFunds([FromBody] DepositFundsDto dto)
    {
        var userId = HttpContext.GetUserId();

        var result = await _transactionService.DepositFundsAsync(userId, dto);
        if (!result.Success)
            return BadRequest(new { Message = result.ErrorMessage });

        return Ok(new { TransactionId = result.Data });
    }

    /// <summary>
    /// Yeni bir para transferi yapar.
    /// </summary>
    [HttpPost("transfer")]
    public async Task<IActionResult> TransferFunds([FromBody] TransferFundsDto dto)
    {
        var userId = HttpContext.GetUserId();

        var result = await _transactionService.TransferFundsAsync(userId, dto);
        if (!result.Success)
        {
            return BadRequest(new
            {
                Success = false,
                Message = result.ErrorMessage
            });
        }

        return Ok(new
        {
            Success = true,
            TransactionId = result.Data
        });
    }
}
