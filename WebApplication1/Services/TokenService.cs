using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SafeScribe.Data;
using SafeScribe.Dtos;
using SafeScribe.Models;

namespace SafeScribe.Services
{
	/// Implementação do serviço de autenticação/registro e emissão de JWTs.
	public class TokenService : ITokenService
	{
		private readonly AppDbContext _db;
		private readonly JwtSettings _jwtSettings;

		public TokenService(AppDbContext db, IOptions<JwtSettings> jwtOptions)
		{
			_db = db;
			_jwtSettings = jwtOptions.Value;
		}


		/// Registra um novo usuário.
		/// Usa BCrypt para hash da senha e garante unicidade do username.
		/// Workaround Oracle: evita AnyAsync por gerar SQL com literal booleano.
		public async Task<User> RegisterAsync(UserRegisterDto dto)
		{
			var usernameNorm = dto.Username.Trim();
			// Workaround para Oracle: evitar AnyAsync (pode gerar 'FALSE' em SQL)
			var existing = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == usernameNorm);
			if (existing != null)
				throw new InvalidOperationException("Username já existe");

			var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

			var user = new User
			{
				Username = usernameNorm,
				PasswordHash = passwordHash,
				Role = string.IsNullOrWhiteSpace(dto.Role) ? RoleNames.Reader : dto.Role
			};

			_db.Users.Add(user);
			await _db.SaveChangesAsync();
			return user;
		}

		/// Autentica o usuário e retorna um token JWT válido.
		public async Task<string> LoginAsync(LoginRequestDto dto)
		{
			var usernameNorm = dto.Username.Trim();
			var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == usernameNorm && u.IsActive);
			if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
				throw new UnauthorizedAccessException("Credenciais inválidas");

			user.LastLoginAt = DateTime.UtcNow;
			await _db.SaveChangesAsync();

			return GenerateToken(user);
		}

		/// Cria um JWT com claims explicativas:
		/// sub (Id do usuário), unique_name (Username), role (perfil), jti (ID único do token).
		public string GenerateToken(User user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var jti = Guid.NewGuid().ToString();

			// Claims principais do token
			var claims = new[]
			{
				// sub: Identifica o sujeito (usuário) do token
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				// unique_name: Nome de usuário legível
				new Claim(ClaimTypes.Name, user.Username),
				// role: Perfil do usuário no sistema
				new Claim(ClaimTypes.Role, user.Role),
				// jti: Identificador único do token (necessário para blacklist/logout)
				new Claim(JwtRegisteredClaimNames.Jti, jti)
			};

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}


