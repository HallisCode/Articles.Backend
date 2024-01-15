using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserScope
{
	public class User
	{
		public long Id { get; set; }

		[Range(4, 16)]
		public string Nickname { get; set; }

		public DateTime RegistredAt { get; set; }

		[MaxLength(256)]
		public string? Bio { get; set; }

		#region Relationships

		public UserSecurity UserSecurity { get; set; }

		#endregion
	}
}
