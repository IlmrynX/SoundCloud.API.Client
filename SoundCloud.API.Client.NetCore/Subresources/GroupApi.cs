using System;
using System.Collections.Generic;
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
    public class GroupApi : IGroupApi, IPublicGroupApi
    {
        private readonly ISoundCloudRawClient soundCloudRawClient;
        private readonly IPaginationValidator paginationValidator;
        private readonly IGroupConverter groupConverter;
        private readonly IUserConverter userConverter;
        private readonly ITrackConverter trackConverter;
        private readonly string prefix;

        internal GroupApi(
            string groupId,
            ISoundCloudRawClient soundCloudRawClient,
            IPaginationValidator paginationValidator,
            IGroupConverter groupConverter,
            IUserConverter userConverter,
            ITrackConverter trackConverter)
        {
            this.soundCloudRawClient = soundCloudRawClient;
            this.paginationValidator = paginationValidator;
            this.groupConverter = groupConverter;
            this.userConverter = userConverter;
            this.trackConverter = trackConverter;

            prefix = string.Format("groups/{0}", groupId);
        }

        public async Task<SCGroup> GetGroup()
        {
            var group = await soundCloudRawClient.Request<Group>(prefix, string.Empty, HttpMethod.Get);
            return groupConverter.Convert(group);
        }

        public async Task<SCUser[]> GetModerators(int offset, int limit)
        {
            return await GetUsers("moderators", offset, limit);
        }

        public async Task<SCUser[]> GetMembers(int offset, int limit)
        {
            return await GetUsers("members", offset, limit);
        }

        public async Task<SCUser[]> GetContributors(int offset, int limit)
        {
            return await GetUsers("contributors", offset, limit);
        }

        public async Task<SCUser[]> GetUsers(int offset, int limit)
        {
            return await GetUsers("users", offset, limit);
        }

        public async Task<SCTrack[]> GetApprovedTracks(int offset, int limit)
        {
            return await GetTracks("tracks", offset, limit);
        }

        public async Task<SCTrack[]> GetPendingTracks(int offset, int limit)
        {
            return await GetTracks("pending_tracks", offset, limit);
        }

        public async Task<SCTrack> GetPendingTrack(string trackId)
        {
            var track = await soundCloudRawClient.Request<Track>(prefix, string.Format("pending_tracks/{0}", trackId), HttpMethod.Get);
            return trackConverter.Convert(track);
        }

        public async Task AcceptPendingTrack(string trackId)
        {
            await soundCloudRawClient.Request(prefix, string.Format("pending_tracks/{0}", trackId), HttpMethod.Put);
        }

        public async Task RejectPendingTrack(string trackId)
        {
            await soundCloudRawClient.Request(prefix, string.Format("pending_tracks/{0}", trackId), HttpMethod.Delete);
        }

        public async Task<SCTrack[]> GetContributions(int offset, int limit)
        {
            paginationValidator.Validate(offset, limit);
            var tracks = await soundCloudRawClient.Request<Track[]>(prefix, "contributions", HttpMethod.Get, new Dictionary<string, object>().SetPagination(offset, limit), responseType: null);
            return tracks.Select(trackConverter.Convert).ToArray();
        }

        public async Task<SCTrack> GetContribution(string trackId)
        {
            var track = await soundCloudRawClient.Request<Track>(prefix, string.Format("contributions/{0}", trackId), HttpMethod.Get, responseType: null);
            return trackConverter.Convert(track);
        }

        public async Task CreateContribution(string trackId)
        {
            await soundCloudRawClient.Request(prefix, "contributions", HttpMethod.Post, new Dictionary<string, object> { { "track[id]", trackId } });
        }

        public async Task DeleteContribution(string trackId)
        {
            await soundCloudRawClient.Request(prefix, string.Format("contributions/{0}", trackId), HttpMethod.Delete);
        }

        private async Task<SCUser[]> GetUsers(string command, int offset, int limit)
        {
            return await GetEntites<User, SCUser>(command, offset, limit, userConverter.Convert);
        }

        private async Task<SCTrack[]> GetTracks(string command, int offset, int limit)
        {
            return await GetEntites<Track, SCTrack>(command, offset, limit, trackConverter.Convert);
        }

        private async Task<TOut[]> GetEntites<TIn, TOut>(string command, int offset, int limit, Func<TIn, TOut> convert)
        {
            var collection = await soundCloudRawClient.GetCollection<TIn>(paginationValidator, prefix, command, offset, limit);
            return collection.Select(convert).ToArray();
        }
    }
}