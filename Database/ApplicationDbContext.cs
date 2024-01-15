using Database.EntityConfigurations.ArticleScope;
using Database.EntityConfigurations.UserScope;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;

namespace Database
{
	public sealed class ApplicationDbContext : DbContext
	{
		public DbSet<Article> Articles { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<ReviewComment> ReviewComments { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserSecurity> UserSecurity { get; set; }


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserSecurityConfiguration());

			modelBuilder.ApplyConfiguration(new ArticleConfiguration());

			modelBuilder.ApplyConfiguration(new ReviewConfiguration());

			modelBuilder.ApplyConfiguration(new ReviewCommentConfiguration());

			modelBuilder.ApplyConfiguration(new TagConfiguration());
		}

	}
}