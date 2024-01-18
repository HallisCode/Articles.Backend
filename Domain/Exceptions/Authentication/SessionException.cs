namespace Domain.Exceptions.Authentication
{
    public class SessionException : IntentionalInternalException
    {
        public SessionException(string message) : base(message)
        {
        }

        protected override void Initialize()
        {
            StatusCode = 401;
        }
    }
}
