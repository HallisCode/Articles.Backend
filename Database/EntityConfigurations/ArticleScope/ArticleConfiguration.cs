using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Database.EntityConfigurations.ArticleScope
{
	public class ArticleConfiguration : IEntityTypeConfiguration<Article>
	{
		public void Configure(EntityTypeBuilder<Article> builder)
		{
			builder.HasOne<User>(article => article.Author)
				.WithMany(user => user.Articles)
				.HasForeignKey(article => article.AuthorId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(article => article.Title).IsUnique(true);

			builder.Property(article => article.Title).HasMaxLength(512);
		}
	}
}
