using API.Authentication.Attrubites;
using Application.IServices.Authentication;
using Application.Services;
using AspNet.Dto.Request;
using AspNet.Throttle.Attrubites;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[ThrottleResting("authentication", 8, 64)]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService<string, string> authenticationService;


		public AuthenticationController(
			IAuthenticationService<string, string> authenticationService,
			UserService userService,
			IMapper mapper
			)
		{
			this.authenticationService = authenticationService;

		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<string>> LogIn(LogInRequest logInModel)
		{
			string token = await authenticationService.LogInAsync(
				logInModel.Email,
				logInModel.Password
				);

			return token;
		}

		[HttpPost]
		public async Task<ActionResult> LogOut([FromHeader] string sessionId)
		{
			await authenticationService.LogOutAsync(sessionId);

			return Ok();
		}
	}
}
