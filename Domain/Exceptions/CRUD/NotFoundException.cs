namespace Domain.Exceptions.CRUD
{
    public class NotFoundException : IntentionalInternalException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        protected override void Initialize()
        {
            StatusCode = 404;
        }
    }
}
