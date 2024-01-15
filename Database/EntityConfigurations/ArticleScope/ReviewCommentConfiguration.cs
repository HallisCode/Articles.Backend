using Domain.Entities.ArticleScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations.ArticleScope
{
	public class ReviewCommentConfiguration : IEntityTypeConfiguration<ReviewComment>
	{
		public void Configure(EntityTypeBuilder<ReviewComment> builder)
		{
			builder.HasOne<Review>(reviewComment => reviewComment.Review)
				.WithMany(review => review.ReviewComments)
				.HasForeignKey(reviewComment => reviewComment.ReviewId);

			builder.HasAlternateKey(reviewComment => new { reviewComment.ReviewId, reviewComment.UserId });

		}
	}
}
