using Application.IServices;
using AspNet.Authorization.Attrubites;
using AspNet.Dto;
using AspNet.SpecifiedServices;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	private readonly IUserReciever<User> userReciever;

	public AuthorizeAttribute(IUserReciever<User> userReciever)
	{
		this.userReciever = userReciever;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

		if (allowAnonymous) return;

		User? user = userReciever.Get();

		if (user == null)
		{
			SessionException sessionException = new SessionException("SessionId in header isn't found");

			ErrorDetails errorDetails = new ErrorDetails(sessionException.GetType().Name, sessionException.Title);

			context.Result = new JsonResult(errorDetails);
		}
	}
}