using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using AspNet.Attrubites;
using System.Linq;
using AspNet.Dto;
using Domain.Exceptions.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

		if (allowAnonymous) return;

		User? user = (User?) context.HttpContext.Items["User"];

		if (user == null)
		{
			SessionException sessionException = new SessionException("SessionKey isn't found");

			ErrorDetails errorDetails = new ErrorDetails(sessionException.GetType().Name, sessionException.Title);

			context.Result = new JsonResult(errorDetails);
		}
	}
}