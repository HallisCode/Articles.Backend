using Application.ServicesBase.Registry;
using Application.Utils;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.CRUD;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RegistryService : RegistryServiceBase
	{
		private readonly UserSecurityRepository userSecurityRepository;

		private readonly UserRepository userRepository;

		public RegistryService(
			UserSecurityRepository userSecurityRepository,
			UserRepository userRepository)
		{
			this.userSecurityRepository = userSecurityRepository;

			this.userRepository = userRepository;

		}

		/// <summary>
		/// Регистрирует пользователя на основе входных данных. 
		/// Создаются такие сущности как User, UserSecurity.
		/// </summary>
		public override async Task RegistryAsync(string email, string password, string nickname)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				email = SHA256Utils.Encrypt(email, sha256);

				password = SHA256Utils.Encrypt(password, sha256);
			}

			User? user;

			UserSecurity? userSecurity = await userSecurityRepository.TryGetByAsync(email);

			if (userSecurity is null)
			{
				user = await userRepository.TryGetByAsync(nickname);

				if (user is not null) throw new AlreadyExistException("This nickname is already claimed");


				user = await userRepository.CreateAsync(nickname);

				await userSecurityRepository.CreateAsync(email, password, user.Id);
			}
			else
			{
				throw new AlreadyExistException("This email is already registried");
			}
		}
	}
}
