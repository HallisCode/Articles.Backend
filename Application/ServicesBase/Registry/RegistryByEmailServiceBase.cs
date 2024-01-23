using System.Threading.Tasks;
using Application.IServices.Registry;

namespace Application.ServicesBase.Registry
{
    public abstract class RegistryByEmailServiceBase<TRequest> : IRegistryService, IConfirmRegistry<TRequest>
    {
        public abstract Task RegistryAsync(string email, string password, string nickname);

        protected abstract Task SendTokenConfirmRegistryAsync(string email);

        public abstract Task ConfirmRegistryAsync(TRequest token);
    }
}
