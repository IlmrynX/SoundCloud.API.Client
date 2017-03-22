using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Client.Helpers;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Objects.Versioning;

namespace SoundCloud.API.Client.Internal.Versioning.Tracks
{
    internal class V2SearchParametersBuilder : IVersionDependentSearchParametersBuilder
    {
        private const string prefix = "search";
        private const string command = "tracks";

        public SCApiVersion Version { get { return SCApiVersion.V2; } }

        public Func<Dictionary<string, object>, Task<Track[]>> BuildGetter(ISoundCloudRawClient soundCloudRawClient)
        {
            return async p =>
            {
                var a = await soundCloudRawClient.Request<TrackCollection>(prefix, command, HttpMethod.Get, p,
                    responseType: string.Empty, domain: Domain.ApiV2);
                return a.Collection;
            };
        }
    }
}