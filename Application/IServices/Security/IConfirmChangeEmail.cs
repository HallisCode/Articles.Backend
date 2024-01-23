namespace Application.IServices.Security
{
	public interface IConfirmChangeEmail<TRequest> : IConfirmChangeByOldEmail<TRequest>, IConfirmChangeByNewEmail<TRequest>
	{
	}
}
