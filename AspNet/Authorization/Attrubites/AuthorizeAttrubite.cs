using AspNet.Authorization.Attrubites;
using AspNet.Dto;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

		if (allowAnonymous) return;

		User? user = (User?)context.HttpContext.Items["User"];

		if (user == null)
		{
			SessionException sessionException = new SessionException("SessionId in header isn't found");

			ErrorDetails errorDetails = new ErrorDetails(sessionException.GetType().Name, sessionException.Title);

			context.Result = new JsonResult(errorDetails);
		}
	}
}