﻿using Application.IServices;
using AspNet.Dto;
using AspNet.Throttle.Attrubites;
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
			IReadOnlyCollection<Type> handlers,
			IThrottleOptions anonymousPolicy,
			IThrottleOptions authenticatedPolicy
			)
		{
			this.next = next;

			this.memoryCache = memoryCache;


			this.handlers = InitializeHandlers(handlers);

			if (this.handlers.Count == 0) throw new Exception($"Не задан ни один throttle handler для {typeof(ThrottleMiddleware)}");


			this.anonymousPolicy = anonymousPolicy;

			this.authenticatedPolicy = authenticatedPolicy;
		}

		private IReadOnlyCollection<IThrottleHandler> InitializeHandlers(IReadOnlyCollection<Type> handlers)
		{
			List<IThrottleHandler> _handlers = new List<IThrottleHandler>();

			foreach (Type handler in handlers)
			{
				if (!handler.IsAssignableTo(typeof(IThrottleHandler)))
				{
					throw new Exception($"Класс обработчик {handler} должен реализовывать {typeof(IThrottleHandler)}");
				}

				_handlers.Add((IThrottleHandler)Activator.CreateInstance(handler, memoryCache)!);
			}

			return _handlers.AsReadOnly();
		}

		public async Task InvokeAsync(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			};


			string identifier = GetIdentifier(httpContext, userReciever, out bool isAnonymous);

			string policyKey = globalThrottlingKey;

			IThrottleOptions options = isAnonymous ? anonymousPolicy : authenticatedPolicy;


			IThrottleAttribute? throttleAttribute = GetThrottleAttribute(endpoint);

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


			string key = $"{identifier}:{policyKey}";

			bool isThrottle = handler.Throttle(key, options);

			if (isThrottle)
			{
				SendError(httpContext, policyKey);

				return;
			}

			await next.Invoke(httpContext);
		}

		private void SendError(HttpContext httpContext, string policyKey)
		{
			TooManyRequestsException exception = new TooManyRequestsException(
				"Вы совершаете слишком много запросов.",
				$"Вам заблокированы все ресурсы, относящиеся к политике {policyKey}"
				);

			ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

			httpContext.Response.WriteAsJsonAsync(errorDetails);
		}

		private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever, out bool isAnonymous)
		{
			User? user = userReciever.User;

			string key = user?.Id.ToString() ?? httpContext.Request.Host.Host;

			isAnonymous = user is null;

			return key;
		}

		private IThrottleHandler? GetHandler(IThrottleOptions options)
		{
			return handlers.FirstOrDefault(handler => handler.OptionsType == options.GetType());
		}

		private IThrottleAttribute? GetThrottleAttribute(Endpoint endpoint)
		{
			ControllerActionDescriptor? endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

			if (endpointController is null) throw new Exception($"{typeof(ControllerActionDescriptor)} не найден для текущего endpoint");


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;


			IThrottleAttribute? controllerThrottle = controllerType.GetCustomAttributes(false)
				.FirstOrDefault(attribute => attribute is IThrottleAttribute)
				as IThrottleAttribute;

			IThrottleAttribute? actionThrottle = actionType.GetCustomAttributes(false)
				.FirstOrDefault(attribute => attribute is IThrottleAttribute)
				as IThrottleAttribute;

			return actionThrottle ?? controllerThrottle;
		}
	}

	public static class ThrottleMiddlewareExtension
	{
		public static void UseThrottleMiddleware(
			this IApplicationBuilder app,
			IReadOnlyCollection<Type> handlers,
			IThrottleOptions anonymousPolicy,
			IThrottleOptions authenticatedPolicy
			)
		{
			app.UseMiddleware<ThrottleMiddleware>(handlers, anonymousPolicy, authenticatedPolicy);
		}
	}
}
