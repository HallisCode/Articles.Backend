using Domain.Entities.UserScope;

namespace Domain.Entities.ArticleScope
{
	public class ReviewComment
	{
		public long Id { get; set; }

		public string Content { get; set; }


		#region Relationships

		public Review Review { get; set; }

		public long ReviewId { get; set; }

		public User Author { get; set; }

		public long UserId { get; set; }

		#endregion

	}
}

























