using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Core.Primitives;

namespace Infrastructure.EntityConfigurations.Base;

public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(TableName());
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        AddBuilder(builder);
    }

    protected abstract void AddBuilder(EntityTypeBuilder<T> builder);

    protected abstract string TableName();
}

