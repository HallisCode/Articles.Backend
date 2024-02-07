using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public abstract class ThrottleHandlerBase<TOptions> : IThrottleHandler where TOptions : IThrottleOptions
	{
		public readonly Type OptionsType = typeof(TOptions);


		protected readonly IMemoryCache memoryCache;

		public ThrottleHandlerBase(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}


		public virtual bool Throttle(string key, IThrottleOptions options)
		{
			VerifyOptionsType(options);

			TOptions _options = (TOptions)options;

			bool isExistContext = memoryCache.TryGetValue(key, out LimitingContext? context);

			if (!isExistContext)
			{
				context = SetContext(key, _options);
			}

			if (isExistContext && context?.TokensAvailable <= 0)
			{
				ExecuteThrottleRules(_options, context);

				return true;
			}

			context!.TokensAvailable--;

			return false;
		}

		protected abstract void ExecuteThrottleRules(TOptions options, LimitingContext context);

		protected abstract LimitingContext SetContext(string key, TOptions options);

		protected virtual void VerifyOptionsType(object options)
		{
			if (options.GetType() != OptionsType)
			{
				throw new Exception($"Для метода {nameof(VerifyOptionsType)} класса {this.GetType().Name} необходим " +
					$"параметр типа {OptionsType}, а не {options.GetType()}");
			}
		}

		protected class LimitingContext
		{
			public int TokensAvailable { get; set; }

			public LimitingContext(int tokenLimit)
			{
				this.TokensAvailable = tokenLimit;
			}
		}

	}
}
