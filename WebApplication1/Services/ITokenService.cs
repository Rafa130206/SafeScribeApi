using SafeScribe.Dtos;
using SafeScribe.Models;

namespace SafeScribe.Services
{
	/// Serviço responsável por registrar usuários, autenticar e emitir tokens JWT.
	public interface ITokenService
	{
		/// Gera um token JWT para o <paramref name="user"/> com as claims essenciais
		/// (sub, unique_name, role, jti) e o período de expiração configurado.
		/// <param name="user">Usuário autenticado</param>
		/// <returns>Token JWT serializado</returns>
		string GenerateToken(User user);

		/// Registra um novo usuário persistindo o hash da senha (BCrypt) e
		/// validando unicidade de <see cref="User.Username"/>.
		Task<User> RegisterAsync(UserRegisterDto dto);

		/// Valida as credenciais informadas e retorna um token JWT em caso de sucesso.
		Task<string> LoginAsync(LoginRequestDto dto);
	}
}


