using Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyCustomEntityConfiguration();

        modelBuilder.ApplyUtcDateTimeConverter();
    }
}
