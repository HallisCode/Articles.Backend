using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleWindowHandler : ThrottleHandlerBase<ThrottleWindowOptions>
	{
		public ThrottleWindowHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		protected override void ExecuteThrottleRules(string key, ThrottleWindowOptions options, IContext context)
		{
		}

		protected override LimitingWindowContext SetContext(string key, ThrottleWindowOptions options)
		{
			LimitingWindowContext context = new LimitingWindowContext(options.TokenLimit);

			memoryCache.Set(key, context, options.TimeInterval);

			return context;
		}


		protected class LimitingWindowContext : LimitingContext
		{
			public LimitingWindowContext(int tokenLimit) : base(tokenLimit)
			{
			}
		}
	}
}
