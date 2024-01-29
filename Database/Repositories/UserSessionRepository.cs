using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class UserSessionRepository
	{
		private readonly ApplicationDbContext context;

		public UserSessionRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region NullableMethods

		public async Task<UserSession?> TryGetByAsync(string sessionKey)
		{
			UserSession? userSession = await context.UserSessions.AsNoTracking()
				.FirstOrDefaultAsync(userSession => userSession.SessionId == sessionKey);

			return userSession;
		}

		public async Task<UserSession?> TryGetByAsync(long userId)
		{
			UserSession? userSession = await context.UserSessions.AsNoTracking()
				.FirstOrDefaultAsync(userSession => userSession.UserId == userId);

			return userSession;
		}

		#endregion

		#region NotNullableMehdos

		public async Task<UserSession> CreateAsync(string sessionKey, long userId, DateTime expiredAt)
		{
			UserSession userSession = new UserSession(sessionKey, userId, expiredAt);

			context.Add(userSession);

			await context.SaveChangesAsync();

			return userSession;
		}

		/// <summary>
		/// Удаляет сессию по идентификатору.
		/// </summary>
		public async Task DeleteAsync(string sessionId)
		{
			await context.UserSessions.Where(userSessions => userSessions.SessionId == sessionId).ExecuteDeleteAsync();

			await context.SaveChangesAsync();
		}

		/// <summary>
		/// Удаляет все сесси связанные с данным пользователем.
		/// </summary>
		public async Task DeleteAsync(long userId)
		{
			await context.UserSessions.Where(userSession => userSession.UserId == userId).ExecuteDeleteAsync();

			await context.SaveChangesAsync();
		}
	}

	#endregion
}
