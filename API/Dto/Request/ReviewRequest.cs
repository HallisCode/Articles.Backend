using Domain.Enum;

namespace AspNet.Dto.Request
{
	public class ReviewRequest
	{
		public long ArticleId { get; set; }

		public string Content { get; set; }

		public ReviewType Type { get; set; }
	}
}
