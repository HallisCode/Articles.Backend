using System.Threading.Tasks;

namespace Application.IServices
{
	public interface IEmailService
	{
		public Task SendMailAsync(string from, string to, string title, string content);
	}
}
