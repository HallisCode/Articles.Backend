using Domain.Exceptions;
using AspNet.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace AspNet.Validation
{
	public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
	{
		public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
		{
			ErrorDetails errorDetails;

			if (validationProblemDetails is not null)
			{
				ValidationException exception = new ValidationException(
					validationProblemDetails.Title ?? "Данные не прошли валидацию",
					validationProblemDetails.Errors
					);

				errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);
			}
			else
			{
				InternalException exception = new InternalException(
					"Данные не прошли валидацию",
					"Валидатор не отправил данные об ошибке валидации"
					);

				errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title);
			}

			return new BadRequestObjectResult(errorDetails);
		}
	}
}
