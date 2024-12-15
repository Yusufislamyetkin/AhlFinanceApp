using AhlApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AhlApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Hesap detaylarını alır. 
        /// </summary>
        [HttpGet("GetAccountDetails")]
        public async Task<IActionResult> GetAccountDetails()
        {
            var userId = HttpContext.GetUserId();
            var response = await _accountService.GetAccountsByUserIdAsync(userId);
            if (!response.Success)
                return NotFound(new { Message = response.ErrorMessage });

            return Ok(response.Data);
        }
    }
}
