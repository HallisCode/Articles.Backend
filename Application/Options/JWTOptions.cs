namespace Application.Options
{
	public class JWTOptions
	{
		public string SignatureKey { get; set; }

		public int LifeSpanDays { get; set; }
	}
}
