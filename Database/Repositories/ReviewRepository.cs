using Domain.Entities.ArticleScope;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class ReviewRepository
	{
		private readonly ApplicationDbContext context;

		public ReviewRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
		#region NullableMethods

		public async Task<Review?> TryGetBy(long authotrId, long articleId)
		{
			return await context.Reviews.AsNoTracking()
				.FirstOrDefaultAsync(review => review.UserId == authotrId && review.ArticleId == articleId);

		}

		#endregion

		#region NotNullableMethods

		public async Task<Review> CreateAsync(string content, long authorId, long articleId, ReviewType type = ReviewType.netrual)
		{
			Review review = new Review(
				content: content,
				type: type,
				authorId: authorId,
				articleId: articleId
				);

			context.Add(review);

			await context.SaveChangesAsync();

			return review;
		}

		#endregion
	}
}
