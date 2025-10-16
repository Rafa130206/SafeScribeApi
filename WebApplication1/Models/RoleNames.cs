namespace SafeScribe.Models
{
    /// <summary>
    /// Define as funções/perfis dos utilizadores no sistema SafeScribe
    /// </summary>
    public static class RoleNames
    {
        /// <summary>
        /// Leitor: Pode apenas visualizar as suas próprias notas
        /// </summary>
        public const string Reader = "Leitor";
        
        /// <summary>
        /// Editor: Pode criar e editar as suas próprias notas
        /// </summary>
        public const string Editor = "Editor";
        
        /// <summary>
        /// Admin: Possui controle total, podendo visualizar, editar e apagar as notas de qualquer utilizador
        /// </summary>
        public const string Admin = "Admin";
    }
}
