using Application.IServices;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Enum;
using AspNet.Throttle.Options;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;


		private Dictionary<Type, ThrottleDelegate<IThrottleOptions>> handlers;

		private Dictionary<ThrottleGroup, IThrottleOptions> throttleGroupSettings;

		private string globalThrottlingKey = "global";


		public ThrottleMiddleware(RequestDelegate next)
		{
			this.next = next;

			handlers = new Dictionary<Type, ThrottleDelegate<IThrottleOptions>>();

			handlers[typeof(ThrottleWindowOptions)] = null;

			throttleGroupSettings = new Dictionary<ThrottleGroup, IThrottleOptions>()
			{
				{ThrottleGroup.Anonymous, new ThrottleWindowOptions() },
				{ThrottleGroup.Identifier, new ThrottleWindowOptions()}
			};

		}

		public async Task InvokeAsync(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null) await next.Invoke(httpContext);


			string identifier = GetIdentifier(httpContext, userReciever, out bool isAnonymous);

			string key = globalThrottlingKey;

			IThrottleOptions options = isAnonymous ? throttleGroupSettings[ThrottleGroup.Anonymous] : throttleGroupSettings[ThrottleGroup.Identifier];


			IThrottleAttribute<IThrottleOptions>? throttleAttribute = GetThrottleAttribute(endpoint);

			if (throttleAttribute is not null)
			{
				options = throttleAttribute.GetOptions();

				key = throttleAttribute.Key;
			}

			ThrottleDelegate<IThrottleOptions>? handler = GetHandler(options);


			await next.Invoke(httpContext);
		}

		private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever, out bool isAnonymous)
		{
			isAnonymous = true;

			string key;


			User? user = userReciever.User;

			if (user is not null)
			{
				isAnonymous = false;

				key = user.Id.ToString();
			}
			else
			{
				key = httpContext.Request.Host.Host;
			}

			return key;
		}

		private ThrottleDelegate<IThrottleOptions>? GetHandler(IThrottleOptions options)
		{
			return handlers.GetValueOrDefault(options.GetType());
		}

		private IThrottleAttribute<IThrottleOptions>? GetThrottleAttribute(Endpoint endpoint)
		{
			ControllerActionDescriptor? endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

			if (endpointController is null) throw new Exception("ControllerActionDescriptor не найден для текущего endpoint");


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;


			IThrottleAttribute<IThrottleOptions>? controllerThrottle = controllerType.GetCustomAttribute<ThrottleWindowAttibute>();

			IThrottleAttribute<IThrottleOptions>? actionThrottle = actionType.GetCustomAttribute<ThrottleWindowAttibute>();

			return actionThrottle ?? controllerThrottle;
		}
	}

	public static class ThrottleMiddlewareExtension
	{
		public static void UseThrottleMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ThrottleMiddleware>();
		}
	}
}
