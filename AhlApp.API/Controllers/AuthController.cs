using AhlApp.Application.DTOs;
using AhlApp.Application.Interfaces;
using AhlApp.Application.Services;
using AhlApp.Shared.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AhlApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// Kullanıcıyı giriş yapar ve JWT token oluşturur.
        /// </summary>
        /// <param name="request">Kullanıcı giriş bilgilerini içeren DTO (email ve şifre).</param>
        /// <returns>Başarılı girişte JWT token, aksi durumda hata mesajı döner.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Response<object>.ErrorResponse(
                    "Geçersiz giriş verisi",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var response = await _authService.AuthenticateAsync(request.Email, request.Password);
            if (!response.Success)
            {
                return Unauthorized(Response<object>.ErrorResponse(response.ErrorMessage));
            }

            return Ok(Response<object>.SuccessResponse(new
            {
                Message = "Giriş başarılı",
                Token = response.Data
            }));
        }

        /// <summary>
        /// Yeni bir kullanıcı kaydeder.
        /// </summary>
        /// <param name="userDto">Kayıt bilgilerini içeren DTO (isim, email, şifre).</param>
        /// <returns>Kayıt başarılıysa kullanıcı bilgileri, aksi durumda hata mesajı döner.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Geçersiz kayıt verisi",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var result = await _userService.RegisterUserAsync(userDto.Name, userDto.Email, userDto.Password);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }
            return Ok(new
            {
                Message = "Kullanıcı başarıyla kaydedildi",
                UserDetails = result.Data
            });
        }
    }
}
