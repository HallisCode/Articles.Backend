using Application.IServices.Registry;
using System.Threading.Tasks;

namespace Application.ServicesBase.Registry
{
	public abstract class RegistryByEmailServiceBase<TRequest> : IRegistryService, IConfirmRegistry<TRequest>
	{
		public abstract Task RegistryAsync(string email, string password, string nickname);

		protected abstract Task SendTokenConfirmRegistryAsync(string email);

		public abstract Task ConfirmRegistryAsync(TRequest token);
	}
}
