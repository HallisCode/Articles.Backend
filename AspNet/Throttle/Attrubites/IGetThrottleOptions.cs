using AspNet.Throttle.Options;

namespace AspNet.Throttle.Attrubites
{
    public interface IGetThrottleOptions<out TOptions> where TOptions : IThrottleOptions
	{
		public TOptions GetOptions();
	}
}
