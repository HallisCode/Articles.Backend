﻿using API.Options;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Handlers;
using AspNet.Throttle.Options;
using Domain.Entities.UserScope;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;


		private readonly IReadOnlyCollection<IThrottleHandler> handlers;

		private readonly IThrottleOptions authenticatedPolicy;

		private readonly IThrottleOptions anonymousPolicy;

		private readonly string globalThrottlingKey = "global";


		public ThrottleMiddleware(
			RequestDelegate next,
			IMemoryCache memoryCache,
			ThrottleMiddlewareOptions options
		)
		{
			this.next = next;

			this.memoryCache = memoryCache;


			this.handlers = InitializeHandlers(options.handlers);

			if (this.handlers.Count == 0) throw new NullReferenceException("Не задан ни один throttle handler.");

			if (options.anonymousPolicy is null) throw new NullReferenceException("Не задана anonymousPolicy.");

			if (options.authenticatedPolicy is null) throw new NullReferenceException("Не задана authenticatedPolicy.");


			this.anonymousPolicy = options.anonymousPolicy;

			this.authenticatedPolicy = options.authenticatedPolicy;
		}

		/// <summary>
		/// Добавляет все переданные обработчики, проверя их на соответствие сигнатуре.
		/// </summary>
		/// <param name="handlers">Заданные обработчики.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private IReadOnlyCollection<IThrottleHandler> InitializeHandlers(IEnumerable<Type> handlers)
		{
			List<IThrottleHandler> _handlers = new List<IThrottleHandler>();

			foreach (Type handler in handlers)
			{
				if (!handler.IsAssignableTo(typeof(IThrottleHandler)))
				{
					throw new Exception($"Класс обработчик {handler} должен реализовывать {typeof(IThrottleHandler)}");
				}

				if (handler.GetConstructor(new Type[] { typeof(IMemoryCache) }) is null)
				{
					throw new Exception($"Класс обработчик {handler} должен содержать конструктор, " +
						$"принимающий только 1 аргумент типа {typeof(IMemoryCache)}");
				}

				_handlers.Add((IThrottleHandler)Activator.CreateInstance(handler, memoryCache)!);
			}

			return _handlers.AsReadOnly();
		}

		public async Task InvokeAsync(HttpContext httpContext, IOptions<HttpContextKeys> dataKeys)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();


			// Задаем общие throttle настройки в зависимости от того, аутентифицирован пользователь или нет

			string identifier = GetIdentifier(httpContext, dataKeys, out bool isAnonymous);

			IThrottleOptions options = isAnonymous ? anonymousPolicy : authenticatedPolicy;

			string policyKey = globalThrottlingKey;


			IThrottleAttribute? throttleAttribute = null;

			if (endpoint is not null)
			{
				throttleAttribute = GetThrottleAttribute(endpoint);
			}

			// Меняем throttle настройки, если таковые установлены на ендпоинте

			if (throttleAttribute is not null)
			{
				options = throttleAttribute.GetOptions();

				policyKey = throttleAttribute.Key;
			}


			if (options is null)
			{
				throw new Exception($"Для {typeof(ThrottleMiddleware)} не удалось определить настройки для выполнения throttle инструкций.");
			}

			IThrottleHandler? handler = GetHandler(options);

			if (handler is null)
			{
				throw new Exception($"В {typeof(ThrottleMiddleware)} для настроек типа {options.GetType()} обработчик не задан.");
			}

			// Запускаем выполнение обработчика

			string key = $"{identifier}:{policyKey}";

			bool isThrottle = handler.Throttle(key, options);

			if (isThrottle)
			{
				throw new TooManyRequestsException(
					"Вы совершаете слишком много запросов.",
					$"Вам заблокированы все ресурсы, относящиеся к политике {policyKey}"
				);
			}

			await next.Invoke(httpContext);
		}

		/// <summary>
		/// Возвращает идентификатор пользователя сделавшего запрос.
		/// </summary>
		/// <returns>Идентификатор пользователя</returns>
		private string GetIdentifier(HttpContext httpContext, IOptions<HttpContextKeys> dataKeys, out bool isAnonymous)
		{
			User? user = (User?)httpContext.Items[dataKeys.Value.User];

			string key = user?.Id.ToString() ?? httpContext.Request.Host.Host;

			isAnonymous = user is null;

			return key;
		}


		/// <summary>
		/// Возвращает обработчик в зависимости от типа throttleOptions.
		/// </summary>
		/// <returns>Throttle обработчик</returns>
		private IThrottleHandler? GetHandler(IThrottleOptions options)
		{
			return handlers.FirstOrDefault(handler => handler.OptionsType == options.GetType());
		}

		/// <summary>
		/// Возвращает аттрибут ендпоинта, который содержит throttle настройки.
		/// </summary>
		/// <returns>Throttle аттрибут</returns>
		/// <exception cref="Exception"></exception>
		private IThrottleAttribute? GetThrottleAttribute(Endpoint endpoint)
		{
			ControllerActionDescriptor? endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

			if (endpointController is null) throw new Exception($"{typeof(ControllerActionDescriptor)} не найден для текущего endpoint");


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;


			IThrottleAttribute? actionThrottle = actionType.GetCustomAttributes(false)
				.FirstOrDefault(attribute => attribute is IThrottleAttribute)
				as IThrottleAttribute;

			IThrottleAttribute? controllerThrottle = controllerType.GetCustomAttributes(false)
				.FirstOrDefault(attribute => attribute is IThrottleAttribute)
				as IThrottleAttribute;

			return actionThrottle ?? controllerThrottle;
		}
	}

	public static class ThrottleMiddlewareExtension
	{
		public static void UseThrottleMiddleware(
			this IApplicationBuilder app,
			Action<ThrottleMiddlewareOptions> configureOptions
		)
		{
			ThrottleMiddlewareOptions options = new ThrottleMiddlewareOptions();

			configureOptions(options);

			app.UseMiddleware<ThrottleMiddleware>(options);
		}
	}

	public class ThrottleMiddlewareOptions
	{
		public IThrottleOptions anonymousPolicy { get; set; }

		public IThrottleOptions authenticatedPolicy { get; set; }

		public IEnumerable<Type> handlers { get; set; }
	}
}
