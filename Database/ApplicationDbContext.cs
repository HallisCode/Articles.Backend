using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;

namespace Database
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Article> Article { get; set; }
		public DbSet<Review> Review { get; set; }
		public DbSet<ReviewComment> ReviewComment { get; set; }
		public DbSet<Tag> Tag { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Security> Security { get; set; }


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}