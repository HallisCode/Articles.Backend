using System.Threading.Tasks;

namespace Application.IServices.Security
{
	/// <summary>
	/// Подтверждает запросы, через токен, полученный на почту.
	/// </summary>
	/// <typeparam name="TRequest"> Представление токена, для подтверждения выполнения запросов.</typeparam>
	public interface IConfirmByEmail<TRequest>
	{
		public Task ConfirmByEmailAsync(TRequest token);
	}
}
