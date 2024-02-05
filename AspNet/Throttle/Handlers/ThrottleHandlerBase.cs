using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public abstract class ThrottleHandlerBase<TOptions> where TOptions : ThrottleOptionsBase
	{
		protected readonly IMemoryCache memoryCache;

		public ThrottleHandlerBase(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}

		public virtual bool Handle(string key, TOptions options)
		{
			LimitingContextBase? context;

			bool isExistContext = memoryCache.TryGetValue<LimitingContextBase>(key, out context);

			if (isExistContext is false)
			{
				context = CreateEntry(key, options);
			}

			if (isExistContext && context.TokensCount <= 0)
			{
				ExecuteRules(key, context);

				return true;
			}

			DecrementTokens(context);

			return false;
		}

		protected abstract LimitingContextBase CreateEntry(string key, TOptions options);

		protected abstract void ExecuteRules(string key, LimitingContextBase context);

		protected virtual void DecrementTokens(LimitingContextBase context)
		{
			context.TokensCount--;
		}

		/// <summary>
		/// Контекст ограничения запросов
		/// </summary>
		protected abstract class LimitingContextBase
		{
			public int TokensCount { get; set; }

			public LimitingContextBase(int tokensCount)
			{
				TokensCount = tokensCount;
			}
		}
	}
}
