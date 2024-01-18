namespace Domain.Entities.UserScope
{
	public class UserSecurity
	{
		public long Id { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }


		#region Relationships

		public User User { get; set; }

		public long UserId { get; set; }

		#endregion


		#region Constructors

		private UserSecurity(string email, string password)
		{
			this.Email = email;

			this.Password = password;
		}

		public UserSecurity(string email, string password, long userId)
		{
			this.Email = email;

			this.Password = password;

			this.UserId = userId;
		}

		#endregion
	}
}
