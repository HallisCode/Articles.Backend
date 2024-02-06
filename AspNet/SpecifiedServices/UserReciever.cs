using Application.IServices;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Http;

namespace AspNet.SpecifiedServices
{
	public class UserReciever : IUserReciever<User>
	{
		public User? User { get; private set; }

		private string key = "User";

		public UserReciever(IHttpContextAccessor httpContextAccessor)
		{
			User = (User?)httpContextAccessor.HttpContext?.Items[key];
		}
	}
}
