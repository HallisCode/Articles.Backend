using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations.UserScope
{
	public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
	{
		public void Configure(EntityTypeBuilder<UserSession> builder)
		{
			builder.HasOne<User>(userSession => userSession.User)
				.WithMany(user => user.UserSessions)
				.HasForeignKey(userSession => userSession.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(userSession => userSession.SessionKey).IsUnique(true);

			builder.Property(userSession => userSession.SessionKey).HasMaxLength(128);

		}
	}
}
