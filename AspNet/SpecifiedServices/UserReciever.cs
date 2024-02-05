using Application.IServices;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Http;

namespace AspNet.SpecifiedServices
{
	public class UserReciever : IUserReciever<User>
	{
		private string key = "User";

		private User? user;

		public UserReciever(HttpContext httpContext)
		{
			user = (User?) httpContext.Items[key];
		}

		public User? Get()
		{
			return user;
		}

		public void Set(User user)
		{
			this.user = user;
		}
	}
}
