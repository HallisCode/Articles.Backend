using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
			UserSession? session = await context.UserSessions.AsNoTracking()
				.FirstOrDefaultAsync(session => session.Token == token);

			await UpdateActivity(session);

			return session;
		}

		public async Task<UserSession?> TryGetByAsync(long userId, string appName)
		{
			UserSession? session = await context.UserSessions.AsNoTracking()
				.Where(session => session.UserId == userId)
				.FirstOrDefaultAsync(session => session.AppName == appName);

			await UpdateActivity(session);

			return session;
		}

		public async Task<List<UserSession>> TryGetAllByAsync(long userId)
		{
			List<UserSession>? sessions = await context.UserSessions.AsNoTracking()
				.Where(session => session.UserId == userId).ToListAsync();

			return sessions;
		}

		#endregion

		#region NotNullableMehdos

		public async Task<UserSession> CreateAsync(long userId, string token, string appName)
		{
			UserSession? session = new UserSession(userId, token, appName);

			context.Add(session);


			await context.SaveChangesAsync();

			return session;
		}

		public async Task DeleteAsync(UserSession session)
		{
			context.Remove(session);

			await context.SaveChangesAsync();
		}

		public async Task DeleteAllElseAsync(UserSession session)
		{
			List<UserSession> sessions = await context.UserSessions.Where(_session => _session.UserId == session.UserId)
				.Where(_session => _session.Id != session.Id)
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

		private async Task UpdateActivity(UserSession? session)
		{
			if (session is not null)
			{
				session.LastActivity = DateTime.UtcNow;

				await context.SaveChangesAsync();
			}
		}
	}
}
