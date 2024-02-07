using AspNet.Throttle.Options;

namespace AspNet.Throttle.Attrubites
{
	public interface IThrottleAttribute<out TOptions> : IThrottleOptions, IGetThrottleOptions<TOptions> where TOptions : IThrottleOptions
	{

	}
}
