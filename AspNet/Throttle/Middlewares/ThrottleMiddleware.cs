﻿using AspNet.Dto;
using AspNet.Throttle.Attrubites;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	/// <summary>
	/// <para>Миддлварь для контроля за количеством запросов во времени к серверу (request/time).</para>
	/// <para>Задаёт ограничения на основе параметров, пока не попадутся заданные значения, по схеме : controller attribute -> class attribute-> general settings</para>
	/// </summary>
	public sealed class ThrottleMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IMemoryCache memoryCache;

        private readonly ThrottleMiddlewareOptions options;


        private const string globalThrottleKey = "general";


        public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache, ThrottleMiddlewareOptions options)
        {
            this.next = next;

            this.memoryCache = memoryCache;

            this.options = options;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (options is null) throw new Exception("ThrottleMiddlewareOptions is null");


            Endpoint? endpoint = httpContext.GetEndpoint();

            if (endpoint is null)
            {
                await next.Invoke(httpContext);

                return;
			} 


            ControllerActionDescriptor endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()!;

			TypeInfo typeOwnerController = endpointController.ControllerTypeInfo;

			MethodInfo typeController = endpointController.MethodInfo;


			RateLimitAttribute? throttleOwner =  typeOwnerController.GetCustomAttribute<RateLimitAttribute>();

            RateLimitAttribute? throttleController = typeController.GetCustomAttribute<RateLimitAttribute>();


            string additionalKey;

            int tokenCounts;

            TimeSpan timeSpan;

			if (throttleController is not null)
            {
                additionalKey = throttleController.Key;

                tokenCounts = throttleController.TokenCounts;

                timeSpan = throttleController.TimeSpan;
            }
            else if (throttleOwner is not null)
            {
				additionalKey = throttleOwner.Key;

				tokenCounts = throttleOwner.TokenCounts;

				timeSpan = throttleOwner.TimeSpan;
			}
            else
            {
				additionalKey = globalThrottleKey;

				tokenCounts = options.TokensCount;

				timeSpan = options.TimeSpan;
			}


			string ip = httpContext.Request.Host.ToString();

			string entryKey = $"{ip}:{additionalKey}";

            LimitingContext? limitingContext;

            bool isExistLimitingContext = memoryCache.TryGetValue<LimitingContext>(entryKey, out limitingContext);


            if (isExistLimitingContext is false)
            {
                limitingContext = new LimitingContext(tokenCounts);

                memoryCache.Set(entryKey, limitingContext, timeSpan);
			}

			if (isExistLimitingContext && limitingContext.TokensCount <= 0)
			{
                if (options.isRestingMode) memoryCache.Set(entryKey, limitingContext, timeSpan);

                string details = $"Too much request to : {additionalKey}";

				TooManyRequestsException exception = new TooManyRequestsException("You're doing too much requests", details);

				ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

				await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));

                return;
			}

			limitingContext.TokensCount--;


            await next.Invoke(httpContext);
        }

        /// <summary>
        /// Контекст ограничения запросов
        /// </summary>
        private class LimitingContext
        {
            public int TokensCount { get; set; }

            public LimitingContext(int tokensCount)
            {
                TokensCount = tokensCount;
            }
        }
    }

    public static class ThrottleMiddlewareExtension
    {
        public static void UseThrottleMiddleware(this IApplicationBuilder app, ThrottleMiddlewareOptions options)
        {

            app.UseMiddleware<ThrottleMiddleware>(options);
        }
    }

    /// <summary>
    /// Задаёт общие настройки для всех запросов без параметров
    /// </summary>
    public sealed class ThrottleMiddlewareOptions
    {
        public TimeSpan TimeSpan { get; private set; }

        public int TokensCount { get; private set; }
        
        public bool isRestingMode { get; private set; }

        public ThrottleMiddlewareOptions(int tokensCount, double seconds, bool isRestingMode = false)
        {
            this.TokensCount = tokensCount;

            this.TimeSpan = TimeSpan.FromSeconds(seconds);

            this.isRestingMode = isRestingMode;
        }
    }
}

