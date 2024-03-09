using API.Authentication.Attrubites;
using API.Options;
using Application.Services;
using AspNet.Dto.Request;
using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class ArticleController : ControllerBase
	{
		private readonly ArticleService articleService;

		private readonly IMapper mapper;

		private readonly HttpContextKeys httpContextKeys;


		public ArticleController(ArticleService articleService, IOptions<HttpContextKeys> httpContextKeys, IMapper mapper)
		{
			this.articleService = articleService;

			this.mapper = mapper;

			this.httpContextKeys = httpContextKeys.Value;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<ArticleResponse>> GetById(int id)
		{
			Article article = await articleService.GetByAsync(id);

			return mapper.Map<Article, ArticleResponse>(article);
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<List<ArticleResponse>?>> GetByTitle(string title)
		{
			List<Article>? articles = await articleService.GetByAsync(title);

			return mapper.Map<List<Article>?, List<ArticleResponse>?>(articles);
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<ICollection<ArticleResponse>>> GetByTags([FromQuery] long[] tags)
		{
			List<Article> articles = await articleService.GetByAsync(tags);

			return mapper.Map<List<Article>, List<ArticleResponse>>(articles);
		}

		[HttpPost]
		public async Task<ActionResult<ArticleResponse>> CreateAsync([FromBody] ArticleRequest articleRequest)
		{
			Article article = await articleService.CreateAsync(
				user: (User)HttpContext.Items[httpContextKeys.User]!,
				title: articleRequest.Title,
				content: articleRequest.Content,
				tagsId: articleRequest.Tags
				);

			return mapper.Map<Article, ArticleResponse>(article);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateAsync(long id, [FromBody] ArticleRequest articleRequest)
		{
			await articleService.UpdateAsync(
				user: (User)HttpContext.Items[httpContextKeys.User]!,
				id: id,
				title: articleRequest.Title,
				content: articleRequest.Content,
				tagsId: articleRequest.Tags
				);

			return Ok();
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteAsync(long id)
		{
			await articleService.DeleteAsync(
				user: (User)HttpContext.Items[httpContextKeys.User]!,
				id: id
				);

			return Ok();
		}

	}
}
