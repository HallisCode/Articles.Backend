using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Handlers;
using AspNet.Throttle.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;


		// Нужно сохранить возможность собирать все IThrottleHandler в одной коллекции.
		// Дабы на основе typeof(options) получать конкретный обработчик, для конкретного типа настроек.
		private Dictionary<Type, IThrottleHandler> handlers;

		// private Dictionary<ThrottleGroup, IThrottleOptions> throttleGroupSettings;

		private string globalThrottlingKey = "global";


		public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache)
		{
			this.next = next;

			this.memoryCache = memoryCache;

			handlers = new Dictionary<Type, IThrottleHandler>();

			handlers[typeof(ThrottleWindowOptions)] = new ThrottleWindowHandler(memoryCache);

			//throttleGroupSettings = new Dictionary<ThrottleGroup, IThrottleOptions>()
			//{
			//	{ThrottleGroup.Anonymous, new ThrottleWindowOptions() },
			//	{ThrottleGroup.Identifier, new ThrottleWindowOptions()}
			//};

		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null) await next.Invoke(httpContext);

			string identifier = GetIdentifier(httpContext, out bool isAnonymous);
			// string identifier = GetIdentifier(httpContext, userReciever, out bool isAnonymous);

			string key = globalThrottlingKey;

			IThrottleOptions options = new ThrottleWindowOptions(1, 60);
			// IThrottleOptions options = isAnonymous ? throttleGroupSettings[ThrottleGroup.Anonymous] : throttleGroupSettings[ThrottleGroup.Identifier];


			IThrottleAttribute<IThrottleOptions>? throttleAttribute = GetThrottleAttribute(endpoint);

			if (throttleAttribute is not null)
			{
				options = throttleAttribute.GetOptions();

				key = throttleAttribute.Key;
			}
			// На основе typeof(options), я собираюсь получать характерный обработчик для данных настроек.
			// Конкретным обработчикам, нужны будут конкретные options.
			IThrottleHandler? handler = GetHandler(options);

			if (handler is null)
			{
				await next.Invoke(httpContext);
			}

			// TODO: Если true => выбрасываем исключение.
			bool isThrottle = handler.Throttle(key, options);


			await next.Invoke(httpContext);
		}

		private string GetIdentifier(HttpContext httpContext, out bool isAnonymous)
		{
			isAnonymous = true;

			string key = httpContext.Request.Host.Host;


			return key;
		}

		//private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever, out bool isAnonymous)
		//{
		//	isAnonymous = true;

		//	string key;


		//	User? user = userReciever.User;

		//	if (user is not null)
		//	{
		//		isAnonymous = false;

		//		key = user.Id.ToString();
		//	}
		//	else
		//	{
		//		key = httpContext.Request.Host.Host;
		//	}

		//	return key;
		//}

		private IThrottleHandler? GetHandler(IThrottleOptions options)
		{
			return handlers.GetValueOrDefault(options.GetType());
		}

		private IThrottleAttribute<IThrottleOptions>? GetThrottleAttribute(Endpoint endpoint)
		{
			ControllerActionDescriptor? endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

			if (endpointController is null) throw new Exception("ControllerActionDescriptor не найден для текущего endpoint");


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;

			// Пока конкретные настройки я получаю за счёт конкретного аттрибута, а как быть когда аттррибутов IThrottleHandler будет несколько ?
			// Каким образом я буду получать атрибуты контроллера и метода, которые реализуют IThrottleHandler ?
			// Если через рефлексию, там она будет просто лютейшая, мне кажется я явно реализую не правильно всю архитектуру Throttle в целом.
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
