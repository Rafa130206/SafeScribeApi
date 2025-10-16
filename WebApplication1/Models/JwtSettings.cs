using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Models
{
    /// <summary>
    /// Configurações para o JWT (JSON Web Token)
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Chave secreta usada para assinar e verificar os tokens JWT
        /// Deve ser mantida em segredo e ter pelo menos 256 bits (32 caracteres)
        /// </summary>
        [Required]
        [MinLength(32, ErrorMessage = "A chave JWT deve ter pelo menos 32 caracteres")]
        public string Key { get; set; } = string.Empty;
        
        /// <summary>
        /// Identificador do emissor do token (quem criou o token)
        /// Usado para verificar se o token foi emitido pela aplicação correta
        /// </summary>
        [Required]
        public string Issuer { get; set; } = string.Empty;
        
        /// <summary>
        /// Identificador da audiência do token (para quem o token foi criado)
        /// Usado para verificar se o token é destinado à aplicação correta
        /// </summary>
        [Required]
        public string Audience { get; set; } = string.Empty;
        
        /// <summary>
        /// Tempo de expiração do token em minutos
        /// Após este tempo, o token não será mais válido
        /// </summary>
        [Range(1, 1440, ErrorMessage = "O tempo de expiração deve estar entre 1 e 1440 minutos")]
        public int ExpiresInMinutes { get; set; } = 60;
    }
}

