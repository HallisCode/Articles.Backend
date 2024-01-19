using Database.Repositories;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Domain.Exceptions.CRUD;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ArticleService
	{
		private readonly ArticleRepository articleRepository;

		public ArticleService(ArticleRepository articleRepository)
		{
			this.articleRepository = articleRepository;
		}

		// Get

		/// <summary>
		/// Получаем статью на основе id
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<Article> GetByAsync(long id)
		{

			Article? article = await articleRepository.TryGetByAsync(id);

			if (article is null) throw new NotFoundException("Article is not found");

			return article;
		}

		/// <summary>
		/// Получаем статьи на основе совпадающих тегов
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<List<Article>> GetByAsync(ICollection<string> tags)
		{
			List<Article>? articles = await articleRepository.TryGetByAsync(tags);

			if (articles is null) throw new NotFoundException("Articles is not found");

			return articles;
		}

		// Create

		public async Task<Article> CreateAsync(string title, string content, User user, ICollection<Tag> tags)
		{
			return await articleRepository.CreateAsync(title, content, user, tags);
		}

		// Update

		public async Task<Article> UpdateAsync(Article article, string? title = null, string? content = null, ICollection<Tag>? tags = null)
		{
			return await articleRepository.UpdateAsync(article, title, content, tags);
		}

		// Delete

		public async Task DeleteAsync(Article article)
		{
			await articleRepository.DeleteAsync(article);
		}

	}
}
