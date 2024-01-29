using Application.IServices.Authentication;
using Application.IServices.Registry;
using Application.IServices.Security;
using Application.Services;
using AspNet.Middlewares;
using Database;
using Database.Repositories;
using Domain.Entities.UserScope;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AspNet.Validation;

namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Прочие настройки
			string? connectPostresql = builder.Configuration["ConnectionStrings:Postgresql"];

			#region Services

			// Extensions
			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectPostresql), ServiceLifetime.Scoped);

			builder.Services.AddAutoMapper(typeof(Program).Assembly);

			builder.Services.AddValidatorsFromAssemblyContaining<Program>();
			builder.Services.AddFluentValidationAutoValidation(configuration =>
			{
				configuration.EnableBodyBindingSourceAutomaticValidation = true;
				configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
			});

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

			builder.Services.AddScoped<IRegistryService, RegistryService>();

			#endregion

			WebApplication app = builder.Build();

			// Добавление Swagger
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			#region Middlewares

			app.UseExceptionMiddleware();

			app.UseSessionMiddlewar();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			#endregion

			#region Other

			app.MapControllers();

			app.Run();

			#endregion
		}
	}
}