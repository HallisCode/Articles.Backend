using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
	[Authorize]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly ReviewService reviewService;

		public ReviewController(ReviewService reviewService)
		{
			this.reviewService = reviewService;
		}
	}
}
