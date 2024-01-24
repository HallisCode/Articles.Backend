using System.Threading.Tasks;

namespace Application.IServices.Security
{
	/// <summary>
	/// Подтверждает изменения, на основе токенов, полученные со старой и новой почты.
	/// </summary>
	/// <typeparam name="TRequest"> Представление токена, для подтверждения выполнения запросов.</typeparam>
	public interface IConfirmChangeEmail<TRequest>
	{
		public Task ConfirmChangeOnOldEmailAsync(TRequest token);

		public Task ConfirmChangeOnNewEmailAsync(TRequest token);
	}
}
