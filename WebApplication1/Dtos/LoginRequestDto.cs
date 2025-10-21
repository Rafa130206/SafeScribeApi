using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Dtos
{
	public class LoginRequestDto
	{
		/// Nome de usuário cadastrado.
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Username { get; set; } = string.Empty;

		/// Senha em texto puro para validação. Não é armazenada.
		[Required]
		[StringLength(72, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty;
	}
}


