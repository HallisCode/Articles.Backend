using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Handlers
{
	public interface IThrottleHandler
	{
		public Type OptionsType { get; }

		public bool Throttle(string key, IThrottleOptions options);
	}
}
