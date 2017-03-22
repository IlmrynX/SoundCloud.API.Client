using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Converters;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Internal.Validation;
using SoundCloud.API.Client.Objects;
using SoundCloud.API.Client.Subresources.Helpers;
using SoundCloud.API.Client.Subresources.Public;

namespace SoundCloud.API.Client.Subresources
{
    public class TrackApi : ITrackApi, IPublicTrackApi
    {
        private readonly string trackId;
        private readonly ISoundCloudRawClient soundCloudRawClient;
        private readonly IPaginationValidator paginationValidator;
        private readonly ITrackConverter trackConverter;
        private readonly IUserConverter userConverter;
        private readonly ICommentConverter commentConverter;
        private readonly string prefix;

        internal TrackApi(
            string trackId, 
            ISoundCloudRawClient soundCloudRawClient, 
            IPaginationValidator paginationValidator, 
            ITrackConverter trackConverter, 
            IUserConverter userConverter,
            ICommentConverter commentConverter)
        {
            this.trackId = trackId;
            this.soundCloudRawClient = soundCloudRawClient;
            this.paginationValidator = paginationValidator;
            this.trackConverter = trackConverter;
            this.userConverter = userConverter;
            this.commentConverter = commentConverter;

            prefix = string.Format("tracks/{0}", trackId);
        }

        public async Task<SCTrack> GetTrack()
        {
            var track = await GetInternalTrack();
            return trackConverter.Convert(track);
        }
        
        public async Task UpdateTrack(SCTrack track)
        {
            if (track.Id != trackId)
            {
                throw new SoundCloudApiException(string.Format("Context set for trackId = {0}. Create new context for update another track.", trackId));
            }

            var currentTrack = await GetInternalTrack();

            var diff = currentTrack.GetDiff(trackConverter.Convert(track));

            await soundCloudRawClient.Request(prefix, string.Empty, HttpMethod.Put, diff.ToDictionary(x => string.Format("track[{0}]", x.Key), x => x.Value));
        }

        public async Task DeleteTrack()
        {
            await soundCloudRawClient.Request(prefix, string.Empty, HttpMethod.Delete);
        }

        public async Task<SCComment[]> GetComments(int offset = 0, int limit = 50)
        {
            var comments = await soundCloudRawClient.GetCollection<Comment>(paginationValidator, prefix, "comments", offset, limit);
            return comments.Select(commentConverter.Convert).ToArray();
        }

        public async Task<SCComment> GetComment(string commentId)
        {
            var comment = await soundCloudRawClient.Request<Comment>(prefix, string.Format("comments/{0}", commentId), HttpMethod.Get);
            return commentConverter.Convert(comment);
        }

        public async Task<SCComment> PostComment(string text, TimeSpan? timestamp)
        {
            var parameters = new Dictionary<string, object> { { "comment[body]", text } };
            if (timestamp.HasValue)
            {
                parameters.Add("comment[timestamp]", timestamp.Value.TotalMilliseconds);
            }

            var comment = await soundCloudRawClient.Request<Comment>(prefix, "comments", HttpMethod.Post, parameters);
            return commentConverter.Convert(comment);
        }

        public async Task DeleteComment(string commentId)
        {
            await soundCloudRawClient.Request(prefix, string.Format("comments/{0}", commentId), HttpMethod.Delete);
        }

        public async Task<SCUser[]> GetFavoriters(int offset = 0, int limit = 50)
        {
            return await soundCloudRawClient.GetCollection<SCUser>(paginationValidator, prefix, "favoriters", offset, limit);
        }

        public async Task<SCUser> GetFavoriter(string favoriterId)
        {
            var user = await soundCloudRawClient.Request<User>(prefix, string.Format("favoriters/{0}", favoriterId), HttpMethod.Get);
            return userConverter.Convert(user);
        }

        public async Task<Stream> GetStream()
        {
            return await soundCloudRawClient.RequestStream(prefix, "stream", HttpMethod.Get);
        }

        private async Task<Track> GetInternalTrack()
        {
            return await soundCloudRawClient.Request<Track>(prefix, string.Empty, HttpMethod.Get);
        }
    }
}