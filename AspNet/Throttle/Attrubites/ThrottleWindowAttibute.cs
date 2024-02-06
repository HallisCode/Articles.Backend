using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleWindowAttibute : Attribute, IThrottleAttribute, IGetThrottleOptions<ThrottleWindowOptions>
	{
		public string Key { get; private set; }

		public int TokenLimit { get; private set; }

		public double WindowPeriod { get; private set; }


		public ThrottleWindowOptions GetOptions()
		{
			return new ThrottleWindowOptions();
		}
	}
}
