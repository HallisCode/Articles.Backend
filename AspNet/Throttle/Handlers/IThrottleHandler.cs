using AspNet.Throttle.Options;

namespace AspNet.Throttle.Handlers
{
	public interface IThrottleHandler<TOptions> where TOptions : IThrottleOptions
	{
		public bool Handle(string key, TOptions options);
	}
}
