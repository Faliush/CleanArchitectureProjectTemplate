using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Extentions;

public static class ModelBuilderExtention
{
    private static readonly ValueConverter<DateTime, DateTime> UtcValueConverter =
           new(outside => outside, inside => DateTime.SpecifyKind(inside, DateTimeKind.Utc));

    /// <summary>
    /// Applies the UTC date-time converter to all of the properties that are <see cref="DateTime"/> and end with Utc.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    internal static void ApplyUtcDateTimeConverter(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType mutableEntityType in modelBuilder.Model.GetEntityTypes())
        {
            IEnumerable<IMutableProperty> dateTimeUtcProperties = mutableEntityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) && p.Name.EndsWith("Utc", StringComparison.Ordinal));

            foreach (IMutableProperty mutableProperty in dateTimeUtcProperties)
            {
                mutableProperty.SetValueConverter(UtcValueConverter);
            }
        }
    }
}
