using Domain.Entities.UserScope;
using Domain.Exceptions.CRUD;
using Microsoft.EntityFrameworkCore;
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

		public async Task<User?> TryGetByAsync(long userId)
		{
			User? user = await context.Users.FirstOrDefaultAsync(user => user.Id == userId);

			return user;
		}

		#endregion

		#region NotNullableMehdos

		public async Task<User> GetByAsync(long userId)
		{
			User? user = await TryGetByAsync(userId);

			if (user is null) throw new NotFoundException("User is not found");

			return user;
		}

		public async Task<User?> GetByAsync(string nickname)
		{
			User? user = await context.Users.FirstOrDefaultAsync(user => user.Nickname == nickname);

			return user;
		}


		public async Task<User> CreateAsync(string nickname, string? bio = null)
		{
			User user = new User(nickname, bio);

			context.Add(user);

			await context.SaveChangesAsync();

			return user;
		}

		#endregion

	}
}
