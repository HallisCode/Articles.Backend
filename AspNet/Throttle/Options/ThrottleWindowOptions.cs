using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleWindowOptions : IThrottleOptions
	{
		public string Key => throw new NotImplementedException();

		public int TokenLimit => throw new NotImplementedException();

		public TimeSpan TimeInterval => throw new NotImplementedException();
	}
}
