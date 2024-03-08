using API.Authentication.Middlewares;
using Application.Configs;
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
using Quartz;
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


			// Добавление планировщика задач
			builder.Services.AddScheduledTasks();

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
			builder.Services.AddScoped<UserSessionRepository>();

			// Logic-Services
			builder.Services.AddScoped<ArticleService>();
			builder.Services.AddScoped<ReviewService>();
			builder.Services.AddScoped<UserService>();

			builder.Services.AddScoped<IAuthenticationService<string, string, AuthOptions>, AuthenticationSessionService>();
			builder.Services.AddScoped<IJWTAuthService<User, string>, AuthenticationSessionService>();

			builder.Services.AddScoped<ISecurityService, SecurityService>();

			builder.Services.AddScoped<IRegistryService, RegistryService>();

			// Confugiration
			builder.Services.Configure<SessionConfig>(builder.Configuration.GetSection(nameof(SessionConfig)));

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

			app.UseCors(policy => policy
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod()
			);
			app.UseHttpsRedirection();


			// Собственные

			app.UseExceptionMiddleware();

			app.UseAuthenticationMiddleware();

			app.UseThrottleMiddleware(options =>
			{
				options.anonymousPolicy = new ThrottleSlidingWindowOptions(96, 16, 64);
				options.authenticatedPolicy = new ThrottleSlidingWindowOptions(128, 16, 64);

				options.handlers = new List<Type> {
					typeof(ThrottleWindowHandler),
					typeof(ThrottleRestingHandler),
					typeof(ThrottleSlidingWindowHandler)
				};
			});


			#endregion

			#region Other

			app.MapControllers();

			app.Run();

			#endregion
		}
	}
}