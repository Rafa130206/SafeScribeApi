using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Dtos
{
    public class NoteCreateDto
    {
        /// <summary>
        /// Título da nota
        /// </summary>
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo da nota
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}