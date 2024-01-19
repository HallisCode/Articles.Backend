using Application.Services;
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
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly AuthenticationService authenticationService;

		private readonly IValidator<RegistryDto> registryDtoValidator;

		private IMapper mapper;


		public AuthenticationController(
			AuthenticationService authenticationService,
			IMapper mapper,
			IValidator<RegistryDto> registryDtoValidator
			)
		{
			this.authenticationService = authenticationService;

			this.mapper = mapper;

			this.registryDtoValidator = registryDtoValidator;

		}

		[HttpPost]
		public async Task<ActionResult<UserDto>> Registry(RegistryDto registryDto)
		{
			ValidationResult validationResult = await registryDtoValidator.ValidateAsync(registryDto);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(this.ModelState);

				return BadRequest(this.ModelState);
			}

			User user = await authenticationService.RegistryAsync(
				registryDto.Email,
				registryDto.Password,
				registryDto.Nickname,
				registryDto.Bio
				);

			return mapper.Map<User, UserDto>(user);

		}

		[HttpPost]
		public async Task<ActionResult<string>> LogIn(LogInDto logInDto)
		{
			return await authenticationService.LogInAsync(
				logInDto.Email,
				logInDto.Password
				);
		}

		[HttpPost]
		public async Task<ActionResult> LogOut([FromBody]string sessionKey)
		{
			await authenticationService.LogOutAsync(sessionKey);

			return Ok();
		}
	}
}
