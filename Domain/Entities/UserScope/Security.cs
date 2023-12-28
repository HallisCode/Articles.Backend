using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserScope
{
	public class Security
	{
		public int Id { get; set; }

		[MaxLength(256)]
		public string Email { get; set; }

		[Range(8, 128)]
		public string Password { get; set; }
	}
}
