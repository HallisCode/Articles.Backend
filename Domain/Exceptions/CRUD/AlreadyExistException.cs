namespace Domain.Exceptions.CRUD
{
    public class AlreadyExistException : IntentionalInternalException
    {
        public AlreadyExistException(string message) : base(message)
        {

        }

        protected override void Initialize()
        {
            StatusCode = 409;
        }


    }
}
