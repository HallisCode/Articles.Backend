using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleWindowAttibute : Attribute, IThrottleAttribute<ThrottleWindowOptions>
	{
		public string Key { get; private set; }

		public int TokenLimit { get; private set; }

		public TimeSpan TimeInterval { get; private set; }


		public ThrottleWindowOptions GetOptions()
		{
			return new ThrottleWindowOptions();
		}
	}
}
