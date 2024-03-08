using Application.IServices.Authentication;
using Application.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServicesBase.Authentication
{
	public abstract class AuthentucationSessionServiceBase : IAuthenticationService<string, string, AuthOptions>
	{
		public abstract Task<string> LogInAsync(string email, string password, AuthOptions options);
		public abstract Task LogOutAsync(string token);
	}
}
