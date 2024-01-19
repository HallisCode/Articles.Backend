using AspNet.Dto.Request;
using FluentValidation;

namespace AspNet.Validation.ModelConfiguration 
{
	public class RegistryValidation : AbstractValidator<RegistryDto>
	{
		public RegistryValidation()
		{
			RuleFor(registryDto => registryDto.Nickname).NotEmpty().MinimumLength(6).MaximumLength(32);

			RuleFor(registryDto => registryDto.Email).NotEmpty().MaximumLength(256).EmailAddress();

			RuleFor(registryDto => registryDto.Password).NotEmpty().MinimumLength(8).MaximumLength(64);
		}
	}
}
