using AspNet.Throttle.Options;

namespace AspNet.Throttle.Attrubites
{
	public interface IGetterThrottleOptions<TOptions> where TOptions : IThrottleOptions
	{
		public string Key { get; }

		public TOptions GetOptions();
	}
}
