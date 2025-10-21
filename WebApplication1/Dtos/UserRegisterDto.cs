using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Dtos
{
	public class UserRegisterDto
	{
		/// Nome de usuário único utilizado para autenticação.
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Username { get; set; } = string.Empty;

		/// Senha em texto puro enviada pelo cliente. Será hasheada com BCrypt.
		[Required]
		[StringLength(72, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty;

		/// Perfil do usuário (Reader/Editor/Admin). Padrão: Reader.
		// Opcional: permitir definir a role no cadastro
		public string Role { get; set; } = SafeScribe.Models.RoleNames.Reader;
	}
}


