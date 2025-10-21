namespace SafeScribe.Services
{
	/// Serviço para registrar e consultar tokens revogados via claim jti.
	public interface ITokenBlacklistService
	{
		/// Adiciona o identificador <paramref name="jti"/> à blacklist.
		Task AddToBlacklistAsync(string jti);

		/// Verifica se um <paramref name="jti"/> já foi revogado.
		Task<bool> IsBlacklistedAsync(string jti);
	}
}


