using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication;
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
			UserSecurity? userSecurity = await context.UserSecurity.FirstOrDefaultAsync(userSecurity => userSecurity.Email == email);

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

		#endregion
	}
}
