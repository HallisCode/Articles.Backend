using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations.UserScope
{
    public class SessionConfiguration : IEntityTypeConfiguration<UserSession>
	{
		public void Configure(EntityTypeBuilder<UserSession> builder)
		{

			builder.HasOne<User>(token => token.User)
				.WithMany(user => user.Sessions)
				.HasForeignKey(token => token.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(token => token.Token).IsUnique(true);

			builder.HasAlternateKey(token => new { token.Token, token.AppName });

		}
	}
}
