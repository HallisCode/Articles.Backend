using Domain.Entities.UserScope;
using Domain.Enum;

namespace AspNet.Dto.Response
{
	public class ReviewResponse
	{
		public long Id { get; set; }

		public User Author { get; set; }

		public string Content { get; set; }

		public ReviewType Type { get; set; }
	}
}
