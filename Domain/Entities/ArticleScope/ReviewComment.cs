using Domain.Entities.UserScope;


namespace Domain.Entities.ArticleScope
{
	public class ReviewComment
	{
		public int Id { get; set; }

		public Review Review { get; set; }

		public User Author { get; set; }

		public string Content { get; set; }
	}
}
