using Application.IServices.Authentication;
using Application.Options;
using System.Threading.Tasks;

namespace Application.ServicesBase.Authentication
{
	/// <summary>
	/// Сервис для подтверждения входа на аккаунт через 2FA.
	/// </summary>
	/// <typeparam name="TToken">Тип данных идентифицирующих пользователя.</typeparam>
	/// <typeparam name="TRequest">Представление токена, для подтверждения запросов</typeparam>
	public abstract class AutheticationByEmailServiceBase<TToken, TRequest, IOptions> : IAuthenticationService<TToken, object, IOptions>, IConfirmLogIn<TToken, TRequest>
	{
		public abstract Task<object> LogInAsync(string email, string password, IOptions options);

		protected abstract Task SendTokenConfirmLogInAsync(string email);

		public abstract Task<TToken> ConfirmLogInAsync(TRequest token);

		public abstract Task LogOutAsync(TToken token);
	}
}
