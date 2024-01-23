using System.Threading.Tasks;

namespace Application.IServices
{
	/// <summary>
	/// Сервис для регистрации, подтверждая почту.
	/// </summary>
	/// <typeparam name="TRequest">Принимает какой-то request, на основе которого производит подтверждение регистрации.
	/// В основном это jwt token.
	/// </typeparam>
	public interface IRegistryByEmailService<TRequest> : IRegistryService
	{
		public Task ConfirmRegistryAsync(TRequest token);
	}
}
