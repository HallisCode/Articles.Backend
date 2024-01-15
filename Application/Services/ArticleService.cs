using Database.Repositories;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ArticleService
	{
		private ArticleRepository articleRepository;

		public ArticleService(ArticleRepository articleRepository)
		{
			this.articleRepository = articleRepository;
		}

		// Get

		public async Task<Article?> GetByAsync(long id)
		{
			return await articleRepository.GetByAsync(id);
		}

		public async Task<List<Article>?> GetByAsync(ICollection<Tag> tags)
		{
			return await articleRepository.GetByAsync(tags);
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
