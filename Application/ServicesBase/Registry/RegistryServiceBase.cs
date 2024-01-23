using System.Threading.Tasks;
using Application.IServices.Registry;

namespace Application.ServicesBase.Registry
{
    public abstract class RegistryServiceBase : IRegistryService
    {
        public abstract Task RegistryAsync(string email, string password, string nickname);
    }
}
