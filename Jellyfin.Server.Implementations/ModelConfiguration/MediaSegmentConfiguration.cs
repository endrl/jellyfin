using Jellyfin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jellyfin.Server.Implementations.ModelConfiguration
{
    /// <summary>
    /// FluentAPI configuration for the MediaSegment entity.
    /// </summary>
    public class MediaSegmentConfiguration : IEntityTypeConfiguration<MediaSegment>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<MediaSegment> builder)
        {
            builder
                .Property(s => s.Start)
                .IsRequired();
            builder
                .Property(s => s.End)
                .IsRequired();
            builder
                .Property(s => s.Type)
                .IsRequired();
            builder
                .Property(s => s.TypeIndex)
                .IsRequired();
            builder
                .Property(s => s.ItemId)
                .IsRequired();
            builder
                .Property(s => s.Action)
                .IsRequired();
            builder
                .HasKey(s => new { s.ItemId, s.Type, s.TypeIndex });
            builder
                .HasIndex(s => s.ItemId);

            builder
                .HasOne<MediaSegmentCreator>(x => x.Creator)
                .WithMany(x => x.Segments)
                .HasForeignKey(x => x.CreatorId)
                .HasPrincipalKey(x => x.Creator)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder
                .Navigation(e => e.Creator)
                .IsRequired();
        }
    }
}
