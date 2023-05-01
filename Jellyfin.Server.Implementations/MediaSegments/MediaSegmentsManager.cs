#pragma warning disable CA1307

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jellyfin.Data.Entities;
using Jellyfin.Data.Enums;
using Jellyfin.Data.Events;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.MediaSegments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Server.Implementations.MediaSegments
{
    /// <summary>
    /// Manages the creation and retrieval of <see cref="MediaSegment"/> instances.
    /// </summary>
    public class MediaSegmentsManager : IMediaSegmentsManager
    {
        private readonly IDbContextFactory<JellyfinDbContext> _dbProvider;
        private readonly IUserManager _userManager;
        private readonly ILogger<MediaSegmentsManager> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaSegmentsManager"/> class.
        /// </summary>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="logger">The logger.</param>
        public MediaSegmentsManager(
            IDbContextFactory<JellyfinDbContext> dbProvider,
            IUserManager userManager,
            ILogger<MediaSegmentsManager> logger)
        {
            _dbProvider = dbProvider;
            _userManager = userManager;
            _logger = logger;
        }

        // <inheritdoc/>
        // public event EventHandler<GenericEventArgs<User>>? OnUserUpdated;

        /// <inheritdoc/>
        public async Task<MediaSegment> CreateMediaSegmentAsync(MediaSegment segment)
        {
            var dbContext = await _dbProvider.CreateDbContextAsync().ConfigureAwait(false);
            await using (dbContext.ConfigureAwait(false))
            {
                ValidateSegment(segment);

                var found = dbContext.Segments.Where(s => s.ItemId.Equals(segment.ItemId) && s.Type.Equals(segment.Type) && s.TypeIndex.Equals(segment.TypeIndex)).FirstOrDefault();

                AddOrUpdateSegment(dbContext, segment, found);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            return segment;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MediaSegment>> CreateMediaSegmentsAsync(IEnumerable<MediaSegment> segments)
        {
            var dbContext = await _dbProvider.CreateDbContextAsync().ConfigureAwait(false);
            await using (dbContext.ConfigureAwait(false))
            {
                var foundSegments = dbContext.Segments.Select(s => s);

                foreach (var segment in segments)
                {
                    ValidateSegment(segment);

                    var found = foundSegments.Where(s => s.ItemId.Equals(segment.ItemId) && s.Type.Equals(segment.Type) && s.TypeIndex.Equals(segment.TypeIndex)).FirstOrDefault();

                    AddOrUpdateSegment(dbContext, segment, found);
                }

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            return segments;
        }

        /// <inheritdoc/>
        public List<MediaSegment> GetAllMediaSegments(Guid itemId = default, Guid creatorId = default, MediaSegmentType type = MediaSegmentType.Intro, int typeIndex = -1)
        {
            var allSegments = new List<MediaSegment>();

            var dbContext = _dbProvider.CreateDbContext();
            using (dbContext)
            {
                IQueryable<MediaSegment> queryable = dbContext.Segments.Select(s => s);

                if (!itemId.Equals(default))
                {
                    queryable = queryable.Where(s => s.ItemId.Equals(itemId));
                }

                if (!creatorId.Equals(default))
                {
                    queryable = queryable.Where(s => s.CreatorId.Equals(creatorId));
                }

                if (typeIndex >= 0)
                {
                    queryable = queryable.Where(s => s.Type.Equals(type) && s.TypeIndex.Equals(typeIndex));
                }

                allSegments = queryable.AsNoTracking().ToList();
            }

            return allSegments;
        }

        /// <inheritdoc/>
        public List<MediaSegment> GetAllMediaSegmentsWithAction(Guid userId, Guid itemId)
        {
            var allSegments = new List<MediaSegment>();

            var dbContext = _dbProvider.CreateDbContext();
            using (dbContext)
            {
                // var usr = dbContext.Users.Single(u => u.Id == userId);
                var usr = _userManager.GetUserById(userId);
                if (usr == null)
                {
                    _logger.LogError("User doesn't exist: {0}", userId);
                    throw new ArgumentNullException(nameof(userId));
                }

                // Do not use the query chain of GetAllMediaSegments() to save calls
                var allSegs = dbContext.Segments.Where(s => s.ItemId.Equals(itemId)).AsNoTracking().ToList();

                // apply user action, recommended action or default for segment type
                foreach (var seg in allSegs)
                {
                    var segType = seg.Type;
                    var segRecAct = seg.Action;

                    if (segType == MediaSegmentType.Intro)
                    {
                        seg.Action = usr.SegmentIntroAction == MediaSegmentAction.Auto ? GetRecActionOrDefault(segRecAct, segType) : usr.SegmentIntroAction;
                    }
                    else if (segType == MediaSegmentType.Outro)
                    {
                        seg.Action = usr.SegmentOutroAction == MediaSegmentAction.Auto ? GetRecActionOrDefault(segRecAct, segType) : usr.SegmentOutroAction;
                    }
                    else if (segType == MediaSegmentType.Recap)
                    {
                        seg.Action = usr.SegmentRecapAction == MediaSegmentAction.Auto ? GetRecActionOrDefault(segRecAct, segType) : usr.SegmentRecapAction;
                    }
                    else if (segType == MediaSegmentType.Preview)
                    {
                        seg.Action = usr.SegmentPreviewAction == MediaSegmentAction.Auto ? GetRecActionOrDefault(segRecAct, segType) : usr.SegmentPreviewAction;
                    }
                    else if (segType == MediaSegmentType.Commercial)
                    {
                        seg.Action = usr.SegmentCommercialAction == MediaSegmentAction.Auto ? GetRecActionOrDefault(segRecAct, segType) : usr.SegmentCommercialAction;
                    }
                }
            }

            return allSegments;
        }

        /// <summary>
        /// Get the default action for given type.
        /// <param name="type">The segment type.</param>
        /// </summary>
        /// <returns>The evaluated action.</returns>
        private MediaSegmentAction GetDefaultActionForType(MediaSegmentType type)
        {
            switch (type)
            {
                case MediaSegmentType.Intro:
                    return MediaSegmentAction.Prompt;
                case MediaSegmentType.Outro:
                    return MediaSegmentAction.Prompt;
                case MediaSegmentType.Preview:
                    return MediaSegmentAction.Prompt;
                case MediaSegmentType.Recap:
                    return MediaSegmentAction.Prompt;
                case MediaSegmentType.Commercial:
                    return MediaSegmentAction.Skip;
                default:
                    throw new NotImplementedException("Missing MediaSegmentType->Action default mapping");
            }
        }

        /// <summary>
        /// Add or Update a segment in db.
        /// <param name="dbContext">The db context.</param>
        /// <param name="segment">The segment.</param>
        /// <param name="found">The found segment.</param>
        /// </summary>
        private void AddOrUpdateSegment(JellyfinDbContext dbContext, MediaSegment segment, MediaSegment? found)
        {
            if (found != null)
            {
                found.Start = segment.Start;
                found.End = segment.End;
                found.Action = segment.Action;
            }
            else
            {
                dbContext.Segments.Add(segment);
            }
        }

        /// <summary>
        /// Get the recommended action for given type or default.
        /// </summary>
        /// <param name="recAction">The recommended segment action.</param>
        /// <param name="type">The segment type.</param>
        /// <returns>The MediaSegment action.</returns>
        private MediaSegmentAction GetRecActionOrDefault(MediaSegmentAction recAction, MediaSegmentType type)
        {
            if (recAction == MediaSegmentAction.Auto)
            {
                return GetDefaultActionForType(type);
            }
            else
            {
                return recAction;
            }
        }

        /// <summary>
        /// Validate a segment: itemId, creatorId, start >= end.
        /// </summary>
        /// <param name="segment">The segment to validate.</param>
        private void ValidateSegment(MediaSegment segment)
        {
            if (segment.Start >= segment.End)
            {
                throw new ArgumentException($"start >= end: {segment.Start}>={segment.End} for segment itemId:{segment.ItemId} with type {segment.Type}.{segment.TypeIndex}");
            }

            if (segment.ItemId.Equals(default) || segment.CreatorId.Equals(default))
            {
                throw new ArgumentException($"itemId or creatorId are default: itemId={segment.ItemId}, creatorId={segment.CreatorId} for segment with type {segment.Type}.{segment.TypeIndex}");
            }
        }

        /// <inheritdoc/>
        public async Task<List<MediaSegment>> DeleteSegmentsAsync(Guid itemId = default, Guid creatorId = default, MediaSegmentType type = MediaSegmentType.Intro, int typeIndex = -1)
        {
            var allSegments = new List<MediaSegment>();

            if (creatorId.Equals(default) && itemId.Equals(default))
            {
                throw new ArgumentException($"itemId or creatorId is default: itemId={itemId}, creatorId={creatorId}");
            }

            var dbContext = await _dbProvider.CreateDbContextAsync().ConfigureAwait(false);
            await using (dbContext.ConfigureAwait(false))
            {
                IQueryable<MediaSegment> queryable = dbContext.Segments.Select(s => s);

                if (!itemId.Equals(default))
                {
                    queryable = queryable.Where(s => s.ItemId.Equals(itemId));
                }

                if (!creatorId.Equals(default))
                {
                    queryable = queryable.Where(s => s.CreatorId.Equals(creatorId));
                }

                if (typeIndex >= 0)
                {
                    queryable = queryable.Where(s => s.Type.Equals(type) && s.TypeIndex.Equals(typeIndex));
                }

                allSegments = queryable.AsNoTracking().ToList();

                dbContext.Segments.RemoveRange(allSegments);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            return allSegments;
        }
    }
}
