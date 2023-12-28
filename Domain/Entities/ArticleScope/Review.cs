using Domain.Entities.UserScope;
using Domain.Enum;
using System.ComponentModel;

namespace Domain.Entities.ArticleScope
{
	public class Review
	{
		public int Id { get; set; }

		public Article Article { get; set; }

		public User Author { get; set; }

		public string Content { get; set; }

		[DefaultValue(ReviewType.netrual)]
		public ReviewType Type { get; set; }
	}
}
