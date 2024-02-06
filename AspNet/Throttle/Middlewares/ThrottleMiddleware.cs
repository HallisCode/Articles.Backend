using Application.IServices;
using AspNet.Dto;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Enum;
using AspNet.Throttle.Handlers;
using AspNet.Throttle.Options;
using Domain.Entities.UserScope;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;

		private const string globalThrottleKey = "general";

		// Настройки логики выполнения
		public Dictionary<ThrottleRole, IThrottleOptions> RoleLimits { get; set; }

		public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache, Dictionary<ThrottleRole, IThrottleOptions> roleLimits)
		{
			this.next = next;

			this.memoryCache = memoryCache;

			this.RoleLimits = roleLimits;
		}

		public async Task InvokeAsync(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			}

			IGetterThrottleOptions<IThrottleOptions>? throttleAttribute = GetThrottleAttrubite(endpoint);

			string identifier = GetIdentifier(httpContext, userReciever, out bool isAnonymous);

			string key = (throttleAttribute?.Key) ?? globalThrottleKey;

			string entryKey = $"{identifier}:{key}";


			IThrottleOptions? options = isAnonymous is true ? RoleLimits[ThrottleRole.Anonymous] : RoleLimits[ThrottleRole.Identifier];

			options = throttleAttribute?.GetOptions();

			bool isThrottle = false;


			IThrottleHandler<IThrottleOptions> throttleHandler = GetThrottleHandlerByOptions(options);

			isThrottle = throttleHandler.Handle(entryKey, options);


			if (isThrottle)
			{
				await SendError(httpContext.Response, key);

				return;
			}

			await next.Invoke(httpContext);
		}

		private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever, out bool isAnonymous)
		{
			isAnonymous = false;

			User? user = userReciever.Get();

			if (user is not null)
			{
				isAnonymous = true;

				return user.Id.ToString();
			}

			return httpContext.Request.Host.Host;
		}

		private IThrottleHandler<IThrottleOptions> GetThrottleHandlerByOptions(IThrottleOptions options)
		{
			foreach (IThrottleHandler<IThrottleOptions> handler in Handlers)
			{

				if (handler.GetType().GetGenericArguments()[0] == options.GetType())
				{
					return handler;
				}
			}

			throw new Exception("Необходимый throttle обработчик не найден");
		}

		private IGetterThrottleOptions<IThrottleOptions>? GetThrottleAttrubite(Endpoint endpoint)
		{
			ControllerActionDescriptor endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()!;

			TypeInfo typeController = endpointController.ControllerTypeInfo;

			MethodInfo typeMethod = endpointController.MethodInfo;


			IGetterThrottleOptions<IThrottleOptions>? throttleController = typeController.GetCustomAttribute<ThrottleAttributeBase<IThrottleOptions>>();

			IGetterThrottleOptions<IThrottleOptions>? throttleMethod = typeMethod.GetCustomAttribute<ThrottleAttributeBase<IThrottleOptions>>();

			return throttleMethod ?? throttleController;
		}

		private async Task SendError(HttpResponse httpResponse, string overloadEndpoint)
		{
			string details = $"Too much request to : {overloadEndpoint}";

			TooManyRequestsException exception = new TooManyRequestsException("You're sending too much requests", details);

			ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

			await httpResponse.WriteAsync(JsonSerializer.Serialize(errorDetails));
		}
	}

	public static class ThrottleMiddlewareExtension
	{
		public static void UseThrottleMiddleware(this IApplicationBuilder app, Dictionary<ThrottleRole, IThrottleOptions> roleLimits)
		{
			app.UseMiddleware<ThrottleMiddleware>(roleLimits);
		}
	}
}
