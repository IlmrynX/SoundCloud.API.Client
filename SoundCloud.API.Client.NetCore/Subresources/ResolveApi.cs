using System.Collections.Generic;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Converters;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources
{
    public class ResolveApi : IResolveApi
    {
        private readonly ISoundCloudRawClient soundCloudRawClient;
        private readonly IGroupConverter groupConverter;
        private readonly IUserConverter userConverter;
        private readonly ITrackConverter trackConverter;
        private readonly IPlaylistConverter playlistConverter;

        private const string prefix = "resolve";

        internal ResolveApi(
            ISoundCloudRawClient soundCloudRawClient, 
            IGroupConverter groupConverter, 
            IUserConverter userConverter, 
            ITrackConverter trackConverter, 
            IPlaylistConverter playlistConverter)
        {
            this.soundCloudRawClient = soundCloudRawClient;
            this.groupConverter = groupConverter;
            this.userConverter = userConverter;
            this.trackConverter = trackConverter;
            this.playlistConverter = playlistConverter;
        }

        public async Task<SCUser> GetUser(string url)
        {
            var user = await ResolveUrl<User>(url);
            return userConverter.Convert(user);
        }

        public async Task<SCTrack> GetTrack(string url)
        {
            var track = await ResolveUrl<Track>(url);
            return trackConverter.Convert(track);
        }

        public async Task<SCPlaylist> GetPlaylist(string url)
        {
            var user = await ResolveUrl<Playlist>(url);
            return playlistConverter.Convert(user);
        }

        public async Task<SCGroup> GetGroup(string url)
        {
            var user = await ResolveUrl<Group>(url);
            return groupConverter.Convert(user);
        }

        private async Task<T> ResolveUrl<T>(string url)
            where T : class 
        {
            return await soundCloudRawClient.Request<T>(prefix, string.Empty, HttpMethod.Get, new Dictionary<string, object> { { "url", url } });
        }
    }
}