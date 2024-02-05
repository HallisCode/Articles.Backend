using AspNet.Throttle.Options;

namespace AspNet.Throttle.Attrubites
{
	public interface IThrottleOptions<T> where T : ThrottleOptionsBase
	{
		public T GetOptions();
	}
}
