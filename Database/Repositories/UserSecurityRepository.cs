using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class UserSecurityRepository
	{
		private readonly ApplicationDbContext context;

		public UserSecurityRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region NullableMethods

		public async Task<UserSecurity?> TryGetByAsync(string email)
		{
			UserSecurity? userSecurity = await context.UserSecurity.AsNoTracking()
				.FirstOrDefaultAsync(userSecurity => EF.Functions.ILike(userSecurity.Email, email));

			return userSecurity;
		}

		public async Task<UserSecurity?> TryGetByAsync(long userId)
		{
			UserSecurity? userSecurity = await context.UserSecurity.AsNoTracking()
				.FirstOrDefaultAsync(userSecurity => userSecurity.UserId == userId);

			return userSecurity;
		}

		#endregion

		#region NotNullableMehdos

		public async Task<UserSecurity> CreateAsync(string email, string password, long userId)
		{
			UserSecurity userSecurity = new UserSecurity(email, password, userId);

			context.Add(userSecurity);

			await context.SaveChangesAsync();

			return userSecurity;
		}

		public async Task<UserSecurity> UpdateAsync(UserSecurity userSecurity, string? email = null, string? password = null)
		{
			context.Add(userSecurity);

			if (email is not null) userSecurity.Email = email;

			if (password is not null) userSecurity.Password = password;

			await context.SaveChangesAsync();

			return userSecurity;
		}

		#endregion
	}
}
