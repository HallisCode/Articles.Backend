using AspNet.Throttle.Options;

namespace AspNet.Throttle.Handlers
{
	public interface IThrottleHandler
	{
		public bool Throttle(string key, IThrottleOptions options);
	}
}
