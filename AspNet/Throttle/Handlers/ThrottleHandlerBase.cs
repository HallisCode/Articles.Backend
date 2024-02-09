using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public abstract class ThrottleHandlerBase<TOptions> : IThrottleHandler where TOptions : IThrottleOptions
	{
		public Type OptionsType { get; private set; } = typeof(TOptions);


		protected readonly IMemoryCache memoryCache;


		public ThrottleHandlerBase(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}


		public virtual bool Throttle(string key, IThrottleOptions options)
		{
			VerifyOptionsType(options);

			TOptions _options = (TOptions)options;


			bool isExistContext = memoryCache.TryGetValue(key, out IContext? context);

			if (!isExistContext)
			{
				context = SetContext(key, _options);
			}

			if (isExistContext && context?.TokensAvailable <= 0)
			{
				ExecuteThrottleRules(key, _options, context);

				return true;
			}

			context!.TokensAvailable--;

			return false;
		}

		protected abstract void ExecuteThrottleRules(string key, TOptions options, IContext context);

		protected abstract IContext SetContext(string key, TOptions options);

		protected virtual void VerifyOptionsType(object options)
		{
			if (options.GetType() != OptionsType)
			{
				throw new Exception($"Для метода {nameof(VerifyOptionsType)} класса {this.GetType()} необходим " +
					$"параметр типа {OptionsType}, а не переданного {options.GetType()}");
			}
		}

		protected interface IContext
		{
			public int TokensAvailable { get; set; }
		}

		protected class LimitingContext : IContext
		{
			public int TokensAvailable { get; set; }

			public LimitingContext(int tokenLimit)
			{
				this.TokensAvailable = tokenLimit;
			}
		}

	}
}
