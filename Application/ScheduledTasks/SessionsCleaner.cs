using Application.Configs;
using Database;
using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Scheduler.Jobs
{
    public class SessionCleaner : IJob
    {
        private readonly ApplicationDbContext context;

        private readonly SessionConfig sessionConfig;

        public SessionCleaner(ApplicationDbContext context, IOptions<SessionConfig> sessionConfig)
        {
            this.context = context;

            this.sessionConfig = sessionConfig.Value;
        }

        public async Task Execute(IJobExecutionContext jobContext)
        {
            List<UserSession> sessions = await context.UserSessions.Where(session =>
                session.LastActivity + TimeSpan.FromDays(sessionConfig.PeriodInactivity) <= DateTime.UtcNow)
                .ToListAsync();

            context.RemoveRange(sessions);

            await context.SaveChangesAsync();

        }
	}
}
