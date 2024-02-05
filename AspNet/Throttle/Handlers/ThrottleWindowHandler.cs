using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleWindowHandler : ThrottleHandlerBase<ThrottleWindowOptions>
	{
		public ThrottleWindowHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		protected override LimitingWindowContext CreateEntry(string key, ThrottleWindowOptions options)
		{
			LimitingWindowContext context = new LimitingWindowContext(options.TokenLimit);

			memoryCache.Set<LimitingWindowContext>(key, context, options.WindowPeriod);

			return context;
		}

		protected override void ExecuteRules(string key, LimitingContextBase context, ThrottleWindowOptions options)
		{
			LimitingWindowContext _context = (LimitingWindowContext)context;
		}

		protected class LimitingWindowContext : LimitingContextBase
		{
			public LimitingWindowContext(int tokensCount) : base(tokensCount)
			{
			}
		}
	}
}
