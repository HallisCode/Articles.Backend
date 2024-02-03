using AspNet.Dto.Request;
using FluentValidation;

namespace AspNet.Validation.ModelConfiguration
{
    public class RegistryValidation : AbstractValidator<RegistryRequest>
    {
        public RegistryValidation()
        {
            RuleFor(registry => registry.Nickname).NotEmpty().MinimumLength(6).MaximumLength(32);

            RuleFor(registry => registry.Email).NotEmpty().MaximumLength(256).EmailAddress();

            RuleFor(registry => registry.Password).NotEmpty().MinimumLength(8).MaximumLength(64);
        }
    }
}
