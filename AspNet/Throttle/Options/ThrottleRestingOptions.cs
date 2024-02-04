namespace AspNet.Throttle.Options
{
	public class ThrottleRestingOptions : ThrottleWindowOptions
	{
		public ThrottleRestingOptions(int tokenLimit, double windowPeriod) : base(tokenLimit, windowPeriod)
		{
		}
	}
}
