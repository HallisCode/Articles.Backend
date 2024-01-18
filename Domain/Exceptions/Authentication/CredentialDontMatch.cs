using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Authentication
{
    public class CredentialDontMatch : IntentionalInternalException
    {
        public CredentialDontMatch(string message) : base(message)
        {
        }

        protected override void Initialize()
        {
            StatusCode = 401;
        }
    }
}
