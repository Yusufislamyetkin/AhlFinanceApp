using AhlApp.Application.DTOs.TransactionDTOs;
using AhlApp.Application.Interfaces;
using AhlApp.Domain.Constants;
using AhlApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TransactionReportsController : ControllerBase
{
    private readonly ITransactionReportService _transactionReportService;

    public TransactionReportsController(ITransactionReportService transactionReportService)
    {
        _transactionReportService = transactionReportService;
    }

    /// <summary>
    /// Belirli bir tarih aralığındaki işlemleri getirir.
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>İşlem listesi</returns>
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactionsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = HttpContext.GetUserId();

        var result = await _transactionReportService.GetTransactionsByDateRangeAsync(userId, startDate, endDate);

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
            Data = result.Data
        });
    }


    /// <summary>
    /// Belirli bir dönem için gelir ve gider toplamlarını hesaplar ve aylık rapor oluşturur.
    /// </summary>
    [HttpGet("monthly-summary")]
    public async Task<IActionResult> GetMonthlySummary()
    {
        var userId = HttpContext.GetUserId();

        var result = await _transactionReportService.GetMonthlySummaryAsync(userId);
        if (!result.Success)
            return BadRequest(Response<object>.ErrorResponse(result.ErrorMessage));

        return Ok(Response<object>.SuccessResponse(result.Data));
    }

    /// <summary>
    /// Belirtilen kullanıcı için tarih aralığına ve analiz türüne göre rapor oluşturur.
    /// </summary>
    /// <param name="dateRangeType">Tarih aralığı türü (0: Günlük, 1: Haftalık, 2: Aylık, 3: Yıllık, 4: Tüm Kayıtlar)</param>
    /// <param name="forecastType">Analiz türü (1: Detaylı analiz, 0: Standart özet)</param>
    [HttpGet("summary")]
    public async Task<IActionResult> GetTransactionSummary([FromQuery] int dateRangeType, [FromQuery] int forecastType)
    {
        var userId = HttpContext.GetUserId();

        var result = await _transactionReportService.GetTransactionSummaryAsync(userId, dateRangeType, forecastType);

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
            Data = result.Data
        });
    }



}
