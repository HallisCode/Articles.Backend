using API.Authentication.Middlewares;
using API.Options;
using Application.IServices.Authentication;
using Application.IServices.Registry;
using Application.IServices.Security;
using Application.Options;
using Application.Services;
using AspNet.Middlewares;
using AspNet.Throttle.Handlers;
using AspNet.Throttle.Middlewares;
using AspNet.Throttle.Options;
using AspNet.Validation;
using Database;
using Database.Repositories;
using Domain.Entities.UserScope;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;

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

			// Служебные
			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen();
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddMemoryCache();

			// Библиотека EntityFrameworkCore
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectPostresql), ServiceLifetime.Scoped);

			// Библиотека AutoMapper
			builder.Services.AddAutoMapper(typeof(Program).Assembly);

			// Библиотека FluentValidations
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
			builder.Services.AddScoped<ReviewRepository>();

			// Logic-Services
			builder.Services.AddScoped<ArticleService>();
			builder.Services.AddScoped<ReviewService>();
			builder.Services.AddScoped<UserService>();

			builder.Services.AddScoped<IAuthenticationService<string, string>, AuthenticationJWTService>();
			builder.Services.AddScoped<IJWTAuthService<User, string>, AuthenticationJWTService>();

			builder.Services.AddScoped<ISecurityService, SecurityService>();

			builder.Services.AddScoped<IRegistryService, RegistryService>();

			// Confugiration
			builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(nameof(JWTOptions)));

			#endregion

			WebApplication app = builder.Build();

			// Добавление Swagger
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			#region Middlewares

			// Служебные

			app.UseCors(policy =>
				policy.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod()
			);
			app.UseHttpsRedirection();


			// Собственные

			app.UseExceptionMiddleware();

			app.UseAuthValidatorMiddleware();
			app.UseVerifyNecessaryAuthMiddleware();

			app.UseThrottleMiddleware(
				handlers: new List<Type>()
				{
					typeof(ThrottleWindowHandler),
					typeof(ThrottleRestingHandler),
					typeof(ThrottleSlidingWindowHandler),
				},
				anonymousPolicy: new ThrottleSlidingWindowOptions(96, 16, 64),
				authenticatedPolicy: new ThrottleSlidingWindowOptions(128, 16, 64)
				);

			#endregion

			#region Other

			app.MapControllers();

			app.Run();

			#endregion
		}
	}
}