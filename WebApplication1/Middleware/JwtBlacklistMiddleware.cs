using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SafeScribe.Services;

namespace SafeScribe.Middleware
{
	/// Verifica se o jti do token autenticado está em blacklist e bloqueia a request.
	/// Deve ser executado após <c>UseAuthentication()</c> e antes de <c>UseAuthorization()</c>.
	public class JwtBlacklistMiddleware
	{
		private readonly RequestDelegate _next;
		public JwtBlacklistMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
		{
			var user = context.User;
			if (user?.Identity?.IsAuthenticated == true)
			{
				var jti = user.FindFirstValue(JwtRegisteredClaimNames.Jti);
				if (!string.IsNullOrWhiteSpace(jti))
				{
					var blacklisted = await blacklistService.IsBlacklistedAsync(jti);
					if (blacklisted)
					{
						context.Response.StatusCode = StatusCodes.Status401Unauthorized;
						await context.Response.WriteAsync("Token revogado");
						return;
					}
				}
			}

			await _next(context);
		}
	}
}


