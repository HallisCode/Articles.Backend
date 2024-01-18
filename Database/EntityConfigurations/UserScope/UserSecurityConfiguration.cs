using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations.UserScope
{
	public class UserSecurityConfiguration : IEntityTypeConfiguration<UserSecurity>
	{
		public void Configure(EntityTypeBuilder<UserSecurity> builder)
		{
			builder.HasOne<User>(userSecurity => userSecurity.User)
				.WithOne(user => user.UserSecurity)
				.HasForeignKey<UserSecurity>(userSecurity => userSecurity.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(userSecurity => userSecurity.Email).IsUnique(true);

			builder.Property(userSecurity => userSecurity.Email).HasMaxLength(256);

			builder.Property(userSecurity => userSecurity.Password).HasMaxLength(64);
		}
	}
}
