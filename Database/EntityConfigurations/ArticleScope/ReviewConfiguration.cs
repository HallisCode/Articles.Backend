using Domain.Entities.ArticleScope;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations.ArticleScope
{
	public class ReviewConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.HasOne<Article>(review => review.Article)
				.WithMany(article => article.Reviews)
				.HasForeignKey(review => review.ArticleId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasAlternateKey(review => new { review.ArticleId, review.UserId });

			builder.Property(review => review.Type).HasDefaultValue(ReviewType.netrual);
		}
	}
}
