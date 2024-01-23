using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
	/// <summary>
	/// Отвечает за подтверждение аутрентификации.
	/// </summary>
	/// <typeparam name = "TRequest"> Представление токена, для подтверждения выполнения запросов.</typeparam>
	/// <typeparam name="TToken">Тип данных идентифицирующих пользователя.</typeparam>
	public interface IConfirmLogIn<TToken, TRequest>
	{
		public Task<TToken> ConfirmLogInAsync(TRequest token);
	}
}
