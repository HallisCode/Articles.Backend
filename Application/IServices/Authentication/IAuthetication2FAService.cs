using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
	/// <summary>
	/// Сервис для подтверждения входа на почту через 2FA.
	/// </summary>
	/// <typeparam name="TToken">Тип возвращаемых данных при подтверждении аутентификации. 
	/// Обычно это string, т.к в основном аутентификация реализауется за счёт jwt токенов и sessions.
	/// </typeparam>
	/// <typeparam name="TRequest">Представление токена, для подтверждения запросов</typeparam>
	public interface IAuthetication2FAService<TToken, TRequest> : IAuthenticationService<TToken>
	{
		public Task LogInAsync(string email, string password);

		public Task SendTokenConfirmLogIn(string email, long userId);

		public Task<TToken> ConfirmLogIn(TRequest token);
	}
}
