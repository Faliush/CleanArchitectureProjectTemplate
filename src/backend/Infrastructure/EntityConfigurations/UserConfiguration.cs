using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(user => user.PasswordHash).IsRequired();

        builder.Property(user => user.CreatedOnUtc).IsRequired();

        builder.Property(user => user.ModifiedOnUtc);

        builder.Property(user => user.Deleted);

        builder.Property(user => user.DeletedOnUtc);

        builder.Ignore(user => user.FullName);

        builder.HasMany(x => x.Roles).WithMany(x => x.Users);
    }
}
