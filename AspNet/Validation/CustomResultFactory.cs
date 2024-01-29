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
				errorDetails = new ErrorDetails(
					typeError: "ValidationError",
					title: validationProblemDetails.Title,
					details: validationProblemDetails.Errors
					);
			}
			else
			{
				errorDetails = new ErrorDetails("ValidationError", "validator failure");
			}

			return new BadRequestObjectResult(errorDetails);
		}
	}
}
