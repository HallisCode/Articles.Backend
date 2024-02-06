namespace AspNet.Throttle.Attrubites
{
	public interface IThrottleAttribute
	{
		public string Key { get; }

		public int TokenLimit { get; }

		public double WindowPeriod { get; }
	}
}
