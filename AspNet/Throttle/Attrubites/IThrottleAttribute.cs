using AspNet.Throttle.Options;

namespace AspNet.Throttle.Attrubites
{
	public interface IThrottleAttribute : IThrottleOptions, IGetThrottleOptions
	{
		public string Key { get; }
	}
}
