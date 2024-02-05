using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleRestingAttribute : ThrottleAttributeBase<ThrottleRestingOptions>
	{
		public TimeSpan WindowPeriod { get; private set; }

		public ThrottleRestingAttribute(string key, int tokenLimit, double windowPeriod) : base(key, tokenLimit)
		{
		}

		public override ThrottleRestingOptions GetOptions()
		{
			return new ThrottleRestingOptions(TokenLimit, WindowPeriod.TotalSeconds);
		}
	}
}
