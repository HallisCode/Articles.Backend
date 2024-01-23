using Application.IServices.Registry;
using System.Threading.Tasks;

namespace Application.ServicesBase.Registry
{
	public abstract class RegistryServiceBase : IRegistryService
	{
		public abstract Task RegistryAsync(string email, string password, string nickname);
	}
}
