using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class UserRepository
	{
		private readonly ApplicationDbContext context;

		public UserRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region NullableMethods

		public async Task<User?> TryGetByAsync(long id)
		{
			User? user = await context.Users.AsNoTracking()
				.FirstOrDefaultAsync(user => user.Id == id);

			return user;
		}


		public async Task<User?> TryGetByAsync(string nickname)
		{
			User? user = await context.Users.AsNoTracking()
				.FirstOrDefaultAsync(user => user.Nickname == nickname);

			return user;
		}

		#endregion

		#region NotNullableMehdos

		public async Task<User> CreateAsync(string nickname, string? bio = null)
		{
			User user = new User(nickname, bio);

			user.RegistredAt = DateTime.UtcNow;

			context.Add(user);

			await context.SaveChangesAsync();

			return user;
		}

		#endregion

	}
}
