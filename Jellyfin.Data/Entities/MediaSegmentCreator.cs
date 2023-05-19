#nullable disable
#pragma warning disable CS1591

using System;
using System.Collections.Generic;

namespace Jellyfin.Data.Entities
{
    /// <summary>
    /// Class MediaSegmentCreator to store the CreatorId at another table.
    /// </summary>
    public class MediaSegmentCreator
    {
        /// <summary>
        /// Gets or sets the associated Id for db.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the associated CreatorId.
        /// </summary>
        /// <value>The id.</value>
        public Guid Creator { get; set; }

        /// <summary>
        /// Gets the segments for this creator.
        /// </summary>
        public virtual ICollection<MediaSegment> Segments { get; } = new List<MediaSegment>();
    }
}
