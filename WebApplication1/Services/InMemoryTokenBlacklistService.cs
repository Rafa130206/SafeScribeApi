using System.Collections.Concurrent;

namespace SafeScribe.Services
{
	/// Implementação in-memory (thread-safe) de blacklist de tokens via jti.
	/// Ideal para protótipo; em produção usar cache distribuído (Redis, etc.).
	public class InMemoryTokenBlacklistService : ITokenBlacklistService
	{
		private readonly ConcurrentDictionary<string, byte> _blacklist = new();

		/// <inheritdoc />
		public Task AddToBlacklistAsync(string jti)
		{
			if (!string.IsNullOrWhiteSpace(jti))
				_blacklist[jti] = 1;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<bool> IsBlacklistedAsync(string jti)
		{
			if (string.IsNullOrWhiteSpace(jti)) return Task.FromResult(false);
			return Task.FromResult(_blacklist.ContainsKey(jti));
		}
	}
}


