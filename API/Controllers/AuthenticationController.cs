using API.Authentication.Attrubites;
using API.Dto.Response;
using API.Options;
using Application.IServices.Authentication;
using Application.Options;
using Application.Services;
using AspNet.Dto.Request;
using AspNet.Dto.Response;
using AspNet.Throttle.Attrubites;
using AutoMapper;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[ThrottleResting("authentication", 8, 64)]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService<string, string, AuthOptions> authenticationService;

		private readonly SessionService sessionService;

		private readonly IMapper mapper;


		public AuthenticationController(
			IAuthenticationService<string, string, AuthOptions> authenticationService,
			SessionService sessionService,
			IMapper mapper
		)
		{
			this.authenticationService = authenticationService;

			this.sessionService = sessionService;

			this.mapper = mapper;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<LogedResponse>> LogIn(LogInRequest logInModel)
		{
			string token = await authenticationService.LogInAsync(
				logInModel.Email,
				logInModel.Password,
				new AuthOptions() { AppName = logInModel.AppName }
			);

			User user = await sessionService.GetUserByAsync(token);

			LogedResponse logedResponse = new LogedResponse()
			{
				Token = token,
				User = mapper.Map<User, UserResponse>(user),
			};

			return logedResponse;
		}

		[HttpPost]
		public async Task<ActionResult> LogOut([FromHeader] string sessionId)
		{
			await authenticationService.LogOutAsync(sessionId);

			return Ok();
		}
	}
}
