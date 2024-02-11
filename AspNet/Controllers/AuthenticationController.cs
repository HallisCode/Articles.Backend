﻿using Application.IServices.Authentication;
using AspNet.Authorization.Attrubites;
using AspNet.Dto.Request;
using AspNet.Throttle.Attrubites;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[ThrottleResting("authentication", 8, 64)]
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService<string, string> authenticationService;


		public AuthenticationController(IAuthenticationService<string, string> authenticationService)
		{
			this.authenticationService = authenticationService;

		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<string>> LogIn(LogInRequest logInModel)
		{
			string sessionId = await authenticationService.LogInAsync(
				logInModel.Email,
				logInModel.Password
				);

			return sessionId;
		}

		[HttpPost]
		public async Task<ActionResult> LogOut([FromHeader] string sessionId)
		{
			await authenticationService.LogOutAsync(sessionId);

			return Ok();
		}
	}
}
