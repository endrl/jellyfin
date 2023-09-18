#nullable disable
#pragma warning disable CS1591

using System;
using Jellyfin.Data.Enums;

namespace Jellyfin.Data.Entities
{
    /// <summary>
    /// Class MediaSegment.
    /// </summary>
    public class MediaSegment
    {
        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        /// <value>The start position.</value>
        public double Start { get; set; }

        /// <summary>
        /// Gets or sets the end position.
        /// </summary>
        /// <value>The end position.</value>
        public double End { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The media segment type.</value>
        public MediaSegmentType Type { get; set; }

        /// <summary>
        /// Gets or sets the TypeIndex which relates to the type.
        /// </summary>
        /// <value>The type index.</value>
        public int TypeIndex { get; set; }

        /// <summary>
        /// Gets or sets the associated ItemId.
        /// </summary>
        /// <value>The id.</value>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the associated CreatorId.
        /// </summary>
        /// <value>The id.</value>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the creator recommended action. Can be overwritten with user defined action.
        /// </summary>
        /// <value>The media segment action.</value>
        public MediaSegmentAction Action { get; set; }
    }
}
