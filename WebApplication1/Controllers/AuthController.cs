using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeScribe.Dtos;
using SafeScribe.Services;

namespace SafeScribe.Controllers
{
	[ApiController]
	[Route("api/v1/auth")] 
	/// Endpoints de autenticação: registro, login e logout (com blacklist).
	public class AuthController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly ITokenBlacklistService _blacklist;

		public AuthController(ITokenService tokenService, ITokenBlacklistService blacklist)
		{
			_tokenService = tokenService;
			_blacklist = blacklist;
		}

		[HttpPost("register")]
		[AllowAnonymous]
		/// Cria um novo usuário persistindo hash de senha (BCrypt).
		public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
		{
			var user = await _tokenService.RegisterAsync(dto);
			return Ok(new { user.Id, user.Username, user.Role });
		}

		[HttpPost("login")]
		[AllowAnonymous]
		/// Autentica e retorna o token JWT para uso nas próximas chamadas.
		public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
		{
			var token = await _tokenService.LoginAsync(dto);
			return Ok(new { token });
		}

		[HttpPost("logout")]
		[Authorize]
		/// Revoga o token atual adicionando seu jti à blacklist.
		public async Task<IActionResult> Logout()
		{
			var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
			if (string.IsNullOrWhiteSpace(jti)) return BadRequest("Token sem JTI");
			await _blacklist.AddToBlacklistAsync(jti);
			return Ok(new { message = "Logout realizado" });
		}
	}
}


