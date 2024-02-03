using Application.Services;
using AspNet.Dto.Request;
using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class ReviewController : ControllerBase
	{
		private readonly ReviewService reviewService;

		private readonly IMapper mapper;

		public ReviewController(ReviewService reviewService, IMapper mapper)
		{
			this.reviewService = reviewService;

			this.mapper = mapper;
		}


		[HttpPost]
		public async Task<ActionResult<ReviewResponse>> CreateAsync([FromBody] ReviewRequest reviewRequest)
		{
			Review review = await reviewService.CreateAsync(
				user: (User)HttpContext.Items["User"]!,
				content: reviewRequest.Content,
				articleId: reviewRequest.ArticleId,
				type: reviewRequest.Type
				);

			return mapper.Map<Review, ReviewResponse>(review);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteAsync(long id)
		{
			await reviewService.DeleteAsync(
				user: (User)HttpContext.Items["User"]!,
				id: id
				);

			return Ok();
		}
	}
}
