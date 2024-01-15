using Application.Services;
using AutoMapper;
using Domain.Entities.ArticleScope;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dto.ArticleScope;

namespace WebApi.Controllers
{

	[ApiController]
	[Route("api/[controller]/[action]")]
	public class ArticleController : ControllerBase
	{
		private ArticleService articleService;

		private IMapper mapper;


		public ArticleController(ArticleService articleService, IMapper mapper)
		{
			this.articleService = articleService;

			this.mapper = mapper;
		}

		// Get

		[HttpGet()]
		public async Task<ActionResult<ArticleDto?>> GetById(int id)
		{
			Article? article = await articleService.GetByAsync(id);

			if (article is null) return NotFound();

			return mapper.Map<Article, ArticleDto>(article);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ArticleDto>?>> GetByTags(ICollection<string> tags)
		{
			return NotFound();
		}
	}
}
