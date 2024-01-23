using Application.IServices.Authentication;
using Application.IServices.Security;
using Application.Services;
using AspNet.Middlewares;
using Database;
using Database.Repositories;
using Domain.Entities.UserScope;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Прочие настройки

			string? connectPostresql = builder.Configuration["ConnectionStrings:Postgresql"];


			// Добавление services

			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectPostresql), ServiceLifetime.Scoped);

			builder.Services.AddAutoMapper(typeof(Program).Assembly);
			builder.Services.AddValidatorsFromAssemblyContaining<Program>();

			// Repositories
			builder.Services.AddScoped<ArticleRepository>();
			builder.Services.AddScoped<TagRepository>();
			builder.Services.AddScoped<UserRepository>();
			builder.Services.AddScoped<UserSecurityRepository>();
			builder.Services.AddScoped<UserSessionRepository>();

			// Services
			builder.Services.AddScoped<ArticleService>();

			builder.Services.AddScoped<ISessionService<User, string>, AuthenticationService>();
			builder.Services.AddScoped<IAuthenticationService<string, string>, AuthenticationService>();

			builder.Services.AddScoped<ISecurityService, SecurityService>();


			WebApplication app = builder.Build();


			// Добавление Swagger

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			// Добавление middleware

			app.UseExceptionMiddleware();

			app.UseSessionMiddlewar();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			// Остальное

			app.MapControllers();

			app.Run();
		}
	}
}