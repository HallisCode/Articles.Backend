using Domain.Entities.ArticleScope;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

		public async Task<Review?> TryGetByAsync(long id)
		{
			return await context.Reviews.AsNoTracking()
				.FirstOrDefaultAsync(review => review.Id == id);
		}

		public async Task<Review?> TryGetByAsync(long authotrId, long articleId)
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

			await context.Entry(review).Reference(review => review.Author).LoadAsync();

			return review;
		}

		public async Task<Review> UpdateAsync(Review review, string? content = null, ReviewType? type = null)
		{
			if (content is not null) review.Content = content;

			if (type is not null) review.Type = Enum.Parse<ReviewType>(type.ToString()!);

			context.Add(review);

			await context.SaveChangesAsync();

			return review;
		}

		public async Task DeleteAsync(long id)
		{
			await context.Reviews.Where(review => review.Id == id).ExecuteDeleteAsync();

			await context.SaveChangesAsync();
		}

		#endregion
	}
}
