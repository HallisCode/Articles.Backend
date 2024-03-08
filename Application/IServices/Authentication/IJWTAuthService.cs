using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
	/// <summary>
	/// Сервис для проверки JWT токена
	/// </summary>
	/// <typeparam name="TUser">Класс сущности пользователя</typeparam>
	/// <typeparam name="TJwtToken">Тип jwt токена</typeparam>
	public interface IJWTAuthService<TUser, TJwtToken> where TUser : class
	{
		/// <summary>
		/// Проверяет JWT токен на валидность
		/// </summary>
		/// <returns>Пользователь jwt токена</returns>
		public Task<TUser> VerifySession(TJwtToken jwtToken);
	}
}
