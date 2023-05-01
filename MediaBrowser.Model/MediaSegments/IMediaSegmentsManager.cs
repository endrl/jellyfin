#nullable disable

#pragma warning disable CA1002, CS1591

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Data.Entities;
using Jellyfin.Data.Enums;

namespace MediaBrowser.Model.MediaSegments
{
    public interface IMediaSegmentsManager
    {
        /// <summary>
        /// Create or update a media segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>New MediaSegment.</returns>
        Task<MediaSegment> CreateMediaSegmentAsync(MediaSegment segment);

        /// <summary>
        /// Create multiple new media segment.
        /// </summary>
        /// <param name="segments">List of segments.</param>
        /// <returns>New MediaSegment.</returns>
        Task<IEnumerable<MediaSegment>> CreateMediaSegmentsAsync(IEnumerable<MediaSegment> segments);

        /// <summary>
        /// Get all media segments, skip wanted action. Don't use for media player, see also <see cref="GetAllMediaSegmentsWithAction"/>.
        /// </summary>
        /// <param name="itemId">Optional: Just segments with itemId.</param>
        /// <param name="creatorId">Optional: Just segments with creatorId.</param>
        /// <param name="type">Optional: The segment type. If set requires typeIndex.</param>
        /// <param name="typeIndex">Optional: The typeIndex. If set requires type.</param>
        /// <returns>List of MediaSegment.</returns>
        List<MediaSegment> GetAllMediaSegments(Guid itemId = default, Guid creatorId = default, MediaSegmentType type = MediaSegmentType.Intro, int typeIndex = -1);

        /// <summary>
        /// Get all media segments for itemId with wanted segment action for user. Should be used by player.
        /// </summary>
        /// <param name="userId">Optional: The user guid.</param>
        /// <param name="itemId">Optional: Just segments with itemId.</param>
        /// <returns>List of MediaSegment.</returns>
        List<MediaSegment> GetAllMediaSegmentsWithAction(Guid userId, Guid itemId);

        /// <summary>
        /// Delete Media Segments. Provide an itemId and/or creatorId.
        /// </summary>
        /// <param name="itemId">Optional: The itemId.</param>
        /// <param name="creatorId">Optional: The creatorId.</param>
        /// <param name="type">Optional: The segment type. If set requires typeIndex.</param>
        /// <param name="typeIndex">Optional: The typeIndex. If set requires type.</param>
        /// <returns>Deleted segments.</returns>
        Task<List<MediaSegment>> DeleteSegmentsAsync(Guid itemId = default, Guid creatorId = default, MediaSegmentType type = MediaSegmentType.Intro, int typeIndex = -1);
    }
}
