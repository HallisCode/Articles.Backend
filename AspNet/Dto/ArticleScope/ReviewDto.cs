using Domain.Enum;
using System.ComponentModel;

namespace WebApi.Dto.ArticleScope
{
	public class ReviewDto
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public ReviewType Type { get; set; }
	}
}
