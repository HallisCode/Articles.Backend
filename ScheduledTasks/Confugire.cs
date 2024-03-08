using Microsoft.Extensions.DependencyInjection;
using Quartz.AspNetCore;
using Scheduler.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz
{
	public static class Confugire
	{
		public static IServiceCollection AddScheduledTasks(this IServiceCollection services)
		{
			services.AddQuartz(scheduler =>
			{
				scheduler.AddJob<SessionCleaner>(config => config.WithIdentity(nameof(SessionCleaner)));

				scheduler.AddTrigger(config =>
					config.ForJob(nameof(SessionCleaner))
					.WithSimpleSchedule(options => options.WithIntervalInHours(24).RepeatForever())
				);

			});

			services.AddQuartzServer();

			return services;
		}
	}
}
