using Jellyfin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jellyfin.Server.Implementations.ModelConfiguration
{
    /// <summary>
    /// FluentAPI configuration for the MediaSegmentCreator entity.
    /// </summary>
    public class MediaSegmentCreatorConfiguration : IEntityTypeConfiguration<MediaSegmentCreator>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<MediaSegmentCreator> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
             .Property(s => s.Creator)
             .IsRequired();
        }
    }
}
