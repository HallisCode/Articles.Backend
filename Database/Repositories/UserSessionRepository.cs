using Domain.Entities.UserScope;
using Domain.Exceptions.CRUD;
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
			UserSession? userSession = await context.UserSessions.FirstOrDefaultAsync(userSession => userSession.SessionKey == sessionKey);

			return userSession;
		}

		public async Task<UserSession?> TryGetByAsync(long userId)
		{
			UserSession? userSession = await context.UserSessions.FirstOrDefaultAsync(userSession => userSession.UserId == userId);

			return userSession;
		}

		#endregion

		#region NotNullableMehdos

		#endregion

		public async Task<UserSession> GetByAsync(string sessionKey)
		{
			UserSession? userSession = await TryGetByAsync(sessionKey);

			if (userSession is null) throw new NotFoundException("Session with such a sessionKey isn't exist");

			return userSession;
		}

		public async Task<UserSession> GetByAsync(long userId)
		{
			UserSession? userSession = await TryGetByAsync(userId);

			if (userSession is null) throw new NotFoundException("Session with such an userId isn't exist");

			return userSession;
		}

		public async Task<UserSession> CreateAsync(string sessionKey, long userId, DateTime expiredAt)
		{
			UserSession userSession = new UserSession(sessionKey, userId, expiredAt);

			context.Add(userSession);

			await context.SaveChangesAsync();

			return userSession;
		}

		public async Task DeleteAsync(long id)
		{
			await context.UserSessions.Where(userSessions => userSessions.Id == id).ExecuteDeleteAsync();

			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string sessionKey)
		{
			await context.UserSessions.Where(userSessions => userSessions.SessionKey == sessionKey).ExecuteDeleteAsync();

			await context.SaveChangesAsync();
		}
	}
}
