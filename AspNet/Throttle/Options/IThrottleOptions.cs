using System;

namespace AspNet.Throttle.Options
{
	public interface IThrottleOptions
	{
		public int TokenLimit { get; }

		public TimeSpan TimeInterval { get; }
	}
}
