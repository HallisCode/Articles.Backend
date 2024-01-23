using Application.IServices.Authentication;
using System.Threading.Tasks;

namespace Application.ServicesBase.Authentication
{
	/// <summary>
	/// Сервис для подтверждения входа на почту через 2FA.
	/// </summary>
	/// <typeparam name="TToken">Тип данных идентифицирующих пользователя.</typeparam>
	/// <typeparam name="TRequest">Представление токена, для подтверждения запросов</typeparam>
	public abstract class Authetication2FAServiceBase<TToken, TRequest> : IAuthenticationService<TToken, object>, IConfirmLogIn<TToken, TRequest>
	{
		public abstract Task<object> LogInAsync(string email, string password);

		protected abstract Task SendTokenConfirmLogInAsync(string email);

		public abstract Task<TToken> ConfirmLogInAsync(TRequest token);

		public abstract Task LogOutAsync(TToken token);
	}
}
