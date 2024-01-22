using Application.Services;
using AspNet.Attrubites;
using AspNet.Dto.Request;
using AspNet.Dto.Response;
using AspNet.Validation.Extensions;
using AutoMapper;
using Domain.Entities.UserScope;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly AuthenticationService authenticationService;

		private readonly IValidator<RegistryRequest> registryValidator;

		private IMapper mapper;


		public AuthenticationController(
			AuthenticationService authenticationService,
			IMapper mapper,
			IValidator<RegistryRequest> registryValidator
			)
		{
			this.authenticationService = authenticationService;

			this.mapper = mapper;

			this.registryValidator = registryValidator;

		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<UserResponse>> Registry(RegistryRequest registryRequest)
		{
			ValidationResult validationResult = await registryValidator.ValidateAsync(registryRequest);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(this.ModelState);

				return BadRequest(this.ModelState);
			}

			User user = await authenticationService.RegistryAsync(
				registryRequest.Email,
				registryRequest.Password,
				registryRequest.Nickname,
				registryRequest.Bio
				);

			return mapper.Map<User, UserResponse>(user);

		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<string>> LogIn(LogInRequest logInModel)
		{
			return await authenticationService.LogInAsync(
				logInModel.Email,
				logInModel.Password
				);
		}

		[HttpDelete]
		public async Task<ActionResult> LogOut()
		{
			await authenticationService.LogOutAsync(HttpContext.Request.Headers["sessionKey"]!);

			return Ok();
		}
	}
}
