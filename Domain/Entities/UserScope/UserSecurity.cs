using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserScope
{
	public class UserSecurity
	{
		public long Id { get; set; }

		[MaxLength(256)]
		public string Email { get; set; }

		[Range(8, 128)]
		public string Password { get; set; }


		#region Relationships

		public User User { get; set; }

		public long UserId { get; set; }

		#endregion
	}
}
