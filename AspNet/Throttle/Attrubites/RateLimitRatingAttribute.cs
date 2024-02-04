using AspNet.Throttle.Options;

namespace AspNet.Throttle.Attrubites
{
	public class RateLimitRatingAttribute : RataLimitWindowAttribute
	{
		public RateLimitRatingAttribute(string key, int tokenLimit, double windowPeriod) : base(key, tokenLimit, windowPeriod)
		{
		}

		public override ThrottleRestingOptions GetOptions()
		{
			return new ThrottleRestingOptions(TokenLimit, WindowPeriod.TotalSeconds);
		}
	}
}
