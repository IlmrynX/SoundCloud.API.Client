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
    public class UserApi : IUserApi, IPublicUserApi
    {
        private readonly string userId;
        internal readonly ISoundCloudRawClient soundCloudRawClient;
        internal readonly IPaginationValidator paginationValidator;
        private readonly IUserConverter userConverter;
        private readonly ITrackConverter trackConverter;
        private readonly IPlaylistConverter playlistConverter;
        internal readonly ICommentConverter commentConverter;
        private readonly IGroupConverter groupConverter;
        private readonly IWebProfileConverter webProfileConverter;
        private readonly string prefix;

        internal UserApi(
            string userId, 
            ISoundCloudRawClient soundCloudRawClient, 
            IPaginationValidator paginationValidator, 
            IUserConverter userConverter, 
            ITrackConverter trackConverter, 
            IPlaylistConverter playlistConverter,
            ICommentConverter commentConverter,
            IGroupConverter groupConverter,
            IWebProfileConverter webProfileConverter,
            string customPrefix = null)
        {
            this.userId = userId;
            this.soundCloudRawClient = soundCloudRawClient;
            this.paginationValidator = paginationValidator;
            this.userConverter = userConverter;
            this.trackConverter = trackConverter;
            this.playlistConverter = playlistConverter;
            this.commentConverter = commentConverter;
            this.groupConverter = groupConverter;
            this.webProfileConverter = webProfileConverter;

            prefix = string.IsNullOrEmpty(customPrefix) ? string.Format("users/{0}", userId) : customPrefix;
        }

        public async Task<SCUser> UpdateUser(SCUser user)
        {
            if (!string.IsNullOrEmpty(userId) && user.Id != userId)
            {
                throw new SoundCloudApiException(string.Format("Context set for userId = {0}. Create new context for update another user.", userId));
            }

            var currentUser = await GetInternalUser();
            var diff = currentUser.GetDiff(userConverter.Convert(user));

            var parameters = diff.ToDictionary(x => string.Format("user[{0}]", x.Key), x => x.Value);
            var updatedUser = await soundCloudRawClient.Request<User>(prefix, string.Empty, HttpMethod.Put, parameters);
            return userConverter.Convert(updatedUser);
        }

        public async Task<SCUser> GetUser()
        {
            var user = await GetInternalUser();
            return userConverter.Convert(user);
        }

        public async Task<SCTrack[]> GetTracks(int offset = 0, int limit = 50)
        {
            var tracks = await soundCloudRawClient.GetCollection<Track>(paginationValidator, prefix, "tracks", offset, limit);
            return tracks.Select(trackConverter.Convert).ToArray();
        }

        public async Task<SCPlaylist[]> GetPlaylists(int offset = 0, int limit = 50)
        {
            var playlists = await soundCloudRawClient.GetCollection<Playlist>(paginationValidator, prefix, "playlists", offset, limit);
            return playlists.Select(playlistConverter.Convert).ToArray();
        }

        public async Task<SCUser[]> GetFollowings(int offset = 0, int limit = 50)
        {
            var users = await soundCloudRawClient.GetCollectionBatch<UserCollection, User>(paginationValidator, prefix, "followings", offset, limit);
            return users.Collection.Select(userConverter.Convert).ToArray();
        }

        public async Task<SCUser> GetFollowing(string followingUserId)
        {
            var user = await soundCloudRawClient.Request<User>(prefix, "followings/" + followingUserId, HttpMethod.Get);
            return userConverter.Convert(user);
        }

        public async Task PutFollowing(string followingUserId)
        {
            await soundCloudRawClient.Request(prefix, "followings/" + followingUserId, HttpMethod.Put);
        }

        public async Task DeleteFollowing(string followingUserId)
        {
            await soundCloudRawClient.Request(prefix, "followings/" + followingUserId, HttpMethod.Delete);
        }

        public async Task<SCUser[]> GetFollowers(int offset = 0, int limit = 50)
        {
            var users = await soundCloudRawClient.GetCollectionBatch<UserCollection, User>(paginationValidator, prefix, "followers", offset, limit);
            return users.Collection.Select(userConverter.Convert).ToArray();
        }

        public async Task<SCUser> GetFollower(string followerUserId)
        {
            var user = await soundCloudRawClient.Request<User>(prefix, "followers/" + followerUserId, HttpMethod.Get);
            return userConverter.Convert(user);
        }

        public virtual async Task<SCComment[]> GetComments(int offset = 0, int limit = 50)
        {
            var comments = await soundCloudRawClient.GetCollection<Comment>(paginationValidator, prefix, "comments", offset, limit);
            return comments.Select(commentConverter.Convert).ToArray();
        }

        public async Task<SCTrack[]> GetFavorites(int offset = 0, int limit = 50)
        {
            var tracks = await soundCloudRawClient.GetCollection<Track>(paginationValidator, prefix, "favorites", offset, limit);
            return tracks.Select(trackConverter.Convert).ToArray();
        }

        public async Task<SCTrack> GetFavorite(string favoriteTrackId)
        {
            var track = await soundCloudRawClient.Request<Track>(prefix, "favorites/" + favoriteTrackId, HttpMethod.Get);
            return trackConverter.Convert(track);
        }

        public async Task PutFavorite(string favoriteTrackId)
        {
            await soundCloudRawClient.Request(prefix, "favorites/" + favoriteTrackId, HttpMethod.Put);
        }

        public async Task DeleteFavorite(string favoriteTrackId)
        {
            await soundCloudRawClient.Request(prefix, "favorites/" + favoriteTrackId, HttpMethod.Delete);
        }

        public async Task<SCGroup[]> GetGroups(int offset = 0, int limit = 50)
        {
            var groups = await soundCloudRawClient.GetCollection<Group>(paginationValidator, prefix, "groups", offset, limit);
            return groups.Select(groupConverter.Convert).ToArray();
        }

        public async Task<SCWebProfile[]> GetWebProfiles(int offset = 0, int limit = 50)
        {
            var webProfiles = await soundCloudRawClient.GetCollection<WebProfile>(paginationValidator, prefix, "web-profiles", offset, limit);
            return webProfiles.Select(webProfileConverter.Convert).ToArray();
        }

        private async Task<User> GetInternalUser()
        {
            return await soundCloudRawClient.Request<User>(prefix, string.Empty, HttpMethod.Get);
        }
    }
}