using Application.Services;
using Database;
using Database.Repositories;
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

			builder.Services.AddScoped<ArticleRepository>();
			builder.Services.AddScoped<ArticleService>();



			WebApplication app = builder.Build();


			// Добавление Swagger

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			// Добавление middleware

			app.UseHttpsRedirection();

			app.UseAuthorization();


			// Остальное

			app.MapControllers();

			app.Run();
		}
	}
}