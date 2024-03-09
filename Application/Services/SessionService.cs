using Application.IServices.Authentication;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication;
using System.Threading.Tasks;

namespace Application.Services
{
	public class SessionService : ISessionService<User, string>
	{
		private readonly UserSessionRepository userSessionRepository;

		private readonly UserRepository userRepository;


		public SessionService(UserSessionRepository userSessionRepository, UserRepository userRepository)
		{
			this.userSessionRepository = userSessionRepository;

			this.userRepository = userRepository;
		}

		public async Task<User> GetUserByAsync(string token)
		{
			UserSession? session = await userSessionRepository.TryGetByAsync(token);

			if (session is null)
			{
				throw new SessionException("Указанная сессия не существует");
			}

			User user = (await userRepository.TryGetByAsync(session.UserId))!;

			return user;
		}

		public async Task<User> VerifySession(string token)
		{
			UserSession? session = await userSessionRepository.TryGetByAsync(token);

			if (session is null)
			{
				throw new SessionException("Указанная сессия не существует");
			}

			return (await userRepository.TryGetByAsync(session.UserId))!;
		}
	}
}
