using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleSlidingWindowHandler : ThrottleHandlerBase<ThrottleSlidingWindowOptions>
	{
		public ThrottleSlidingWindowHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		protected override LimitingContextBase CreateEntry(string key, ThrottleSlidingWindowOptions options)
		{
			throw new System.NotImplementedException();
		}

		protected override void ExecuteRules(string key, LimitingContextBase context, ThrottleSlidingWindowOptions options)
		{
			throw new System.NotImplementedException();
		}
	}
}
