﻿using System.Threading.Tasks;
using Application.IServices.Authentication;

namespace Application.ServicesBase.Authentication
{
    /// <summary>
    /// Сервис для аутентификации.
    /// </summary>
    /// <typeparam name="TToken">Тип данных идентифицирующих пользователя.</typeparam>
    public abstract class AutheticationServiceBase<TToken> : IAuthenticationService<TToken, TToken>
    {
        public abstract Task<TToken> LogInAsync(string email, string password);

        public abstract Task LogOutAsync(TToken token);
    }
}
