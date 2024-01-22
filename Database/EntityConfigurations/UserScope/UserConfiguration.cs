using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System;


namespace Database.EntityConfigurations.UserScope
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
		{
			builder.HasIndex(user => user.Nickname).IsUnique(true);

			builder.Property(user => user.Nickname).HasMaxLength(32);

			builder.Property(user => user.Bio).HasMaxLength(256).IsRequired(false);

		}
	}
}
