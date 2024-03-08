using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

		public async Task<UserSession?> TryGetByAsync(string token)
		{
			UserSession? userSession = await context.UserSessions.AsNoTracking()
				.FirstOrDefaultAsync(session => session.Token == token);

			if (userSession is not null) userSession.LastActivity = DateTime.UtcNow;

			return userSession;
		}

		public async Task<UserSession?> TryGetByAsync(long userId, string appName)
		{
			UserSession? userSession = await context.UserSessions.AsNoTracking()
				.Where(sesion => sesion.UserId == userId)
				.FirstOrDefaultAsync(sesion => sesion.AppName == appName);

			if (userSession is not null) userSession.LastActivity = DateTime.UtcNow;

			return userSession;
		}

		#endregion

		#region NotNullableMehdos

		public async Task<UserSession> CreateAsync(long userId, string token, string appName)
		{
			UserSession? userSession = new UserSession(userId, token, appName);

			context.Add(userSession);

			await context.SaveChangesAsync();

			return userSession;
		}

		public async Task DeleteAsync(UserSession session)
		{
			context.Remove(session);

			await context.SaveChangesAsync();
		}

		public async Task DeleteAllElseAsync(UserSession session)
		{
			List<UserSession> sessions = await context.UserSessions.Where(session => session.UserId == session.UserId)
				.Where(session => session.Id != session.Id)
				.ToListAsync();

			context.RemoveRange(sessions);

			await context.SaveChangesAsync();
		}

		public async Task DeleteAllAsync(long userId)
		{
			List<UserSession> sessions = await context.UserSessions.Where(session => session.UserId == userId)
				.ToListAsync();

			context.RemoveRange(sessions);

			await context.SaveChangesAsync();
		}

		#endregion


	}
}
