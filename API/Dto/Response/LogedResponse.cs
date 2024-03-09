using AspNet.Dto.Response;

namespace API.Dto.Response
{
	public class LogedResponse
	{
		public string Token { get; set; }

		public UserResponse User { get; set; }
	}
}
