using System;

namespace AspNet.Throttle.Options
{
	public interface IThrottleOptions
	{
		public string Key { get; }

		public int TokenLimit { get; }

		public TimeSpan TimeInterval { get; }
	}
}
