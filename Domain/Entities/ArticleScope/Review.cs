using Domain.Entities.UserScope;
using Domain.Enum;
using System.Collections.Generic;

namespace Domain.Entities.ArticleScope
{
	public class Review
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public ReviewType Type { get; set; }


		#region Relationships

		public Article Article { get; set; }

		public long ArticleId { get; set; }

		public User Author { get; set; }

		public long UserId { get; set; }

		public ICollection<ReviewComment> ReviewComments { get; set; }

		#endregion
	}
}
