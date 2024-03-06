using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.CRUD;
using System.Threading.Tasks;

namespace Application.Services
{
	public sealed class UserService
	{
		private readonly UserRepository userRepository;


		public UserService(UserRepository userRepository)
		{
			this.userRepository = userRepository;

		}

		/// <summary>
		/// Получаем пользователя на основе id
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<User> GetByAsync(long id)
		{
			User? user = await userRepository.TryGetByAsync(id);

			if (user is null) throw new NotFoundException("Пользователь с таким id не найден");

			return user;

		}

		/// <summary>
		/// Получаем пользователя на основе nickname
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<User> GetByAsync(string nikcname)
		{
			User? user = await userRepository.TryGetByAsync(nikcname);

			if (user is null) throw new NotFoundException("Пользователь с таким никнеймом не найден");

			return user;
		}

	}
}
