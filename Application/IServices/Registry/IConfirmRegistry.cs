using System.Threading.Tasks;

namespace Application.IServices.Registry
{
	public interface IConfirmRegistry<TRequest>
	{
		public Task ConfirmRegistryAsync(TRequest token);
	}
}
