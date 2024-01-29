using Database.Repositories;
using Domain.Entities.ArticleScope;
using Domain.Enum;
using Domain.Exceptions.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ReviewService
	{
		private readonly ReviewRepository reviewRepository;

		public ReviewService(ReviewRepository reviewRepository)
		{
			this.reviewRepository = reviewRepository;
		}

		public async Task<Review> CreateAsync(string content, long authorId, long articleId, ReviewType type = ReviewType.netrual)
		{
			Review? review = await reviewRepository.TryGetBy(authorId, articleId);

			if (review is not null) throw new AlreadyExistException("The same review is already exist");

			review = await reviewRepository.CreateAsync(
				content: content,
				authorId: authorId,
				articleId: articleId,
				type: type
				);


			return review;
		}
	}
}
