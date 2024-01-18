using Domain.Entities.ArticleScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations.ArticleScope
{
	public class TagConfiguration : IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> builder)
		{
			builder.HasMany<Article>(tag => tag.Articles)
				.WithMany(article => article.Tags);

			builder.HasIndex(tag => tag.Title).IsUnique(true);

			builder.Property(tag => tag.Title).HasMaxLength(32);
		}
	}
}
