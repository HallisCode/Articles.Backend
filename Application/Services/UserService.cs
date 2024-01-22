using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.CRUD;
using System.Threading.Tasks;

namespace Application.Services
{
	public class UserService
	{
		private readonly UserRepository userRepository;

		public UserService(UserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		public async Task<User> GetByAsync(long id)
		{
			User? user = await userRepository.TryGetByAsync(id);

			if (user is null) throw new NotFoundException("User with this id isn't found");

			return user;

		}

		public async Task<User> GetByAsync(string nikcname)
		{
			User? user = await userRepository.TryGetByAsync(nikcname);

			if (user is null) throw new NotFoundException("User with this nickname isn't found");

			return user;
		}
	}
}
