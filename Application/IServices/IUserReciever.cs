using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
	public interface IUserReciever<TUser>
	{
		public TUser? Get();

		public void Set(TUser user);
	}
}
