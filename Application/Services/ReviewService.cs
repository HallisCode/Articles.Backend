using Database.Repositories;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Domain.Enum;
using Domain.Exceptions.Authorization;
using Domain.Exceptions.CRUD;
using System.Threading.Tasks;

namespace Application.Services
{
	public sealed class ReviewService
	{
		private readonly ReviewRepository reviewRepository;

		public ReviewService(ReviewRepository reviewRepository)
		{
			this.reviewRepository = reviewRepository;
		}

		/// <summary>
		/// Создаёт review
		/// </summary>
		/// <returns></returns>
		/// <exception cref="AlreadyExistException"></exception>
		public async Task<Review> CreateAsync(User user, string content, long articleId, ReviewType type = ReviewType.netrual)
		{
			Review? review = await reviewRepository.TryGetByAsync(user.Id, articleId);

			if (review is not null) throw new AlreadyExistException("The same review is already exist");

			review = await reviewRepository.CreateAsync(
				content: content,
				authorId: user.Id,
				articleId: articleId,
				type: type
				);


			return review;
		}

		/// <summary>
		/// Обновляет review
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="ArgumentsMissingException"></exception>
		public async Task<Review> UpdateAsync(User user, long id, string content, ReviewType type)
		{
			Review? review = await reviewRepository.TryGetByAsync(id);

			if (review is null) throw new NotFoundException("Review with this id isn't found");

			VerifyIsReviewOwner(user, review);


			return await reviewRepository.UpdateAsync(review, content, type);
		}

		/// <summary>
		/// Удаляет review
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task DeleteAsync(User user, long id)
		{
			Review? review = await reviewRepository.TryGetByAsync(id);

			if (review is null) throw new NotFoundException("Review isn't found");

			VerifyIsReviewOwner(user, review);


			await reviewRepository.DeleteAsync(id);
		}

		/// <summary>
		/// Верификация того, является ли пользовател автором review
		/// </summary>
		/// <exception cref="AccessDeniedException"></exception>
		private void VerifyIsReviewOwner(User user, Review review)
		{
			if (review.UserId != user.Id)
			{
				throw new AccessDeniedException("You aren't author of this review");
			}
		}
	}
}
