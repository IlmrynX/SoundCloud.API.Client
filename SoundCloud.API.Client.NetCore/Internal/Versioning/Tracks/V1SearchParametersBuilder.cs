using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Objects.Versioning;

namespace SoundCloud.API.Client.Internal.Versioning.Tracks
{
    internal class V1SearchParametersBuilder : IVersionDependentSearchParametersBuilder
    {
        private const string prefix = "tracks";

        public SCApiVersion Version => SCApiVersion.V1;

        public Func<Dictionary<string, object>, Task<Track[]>> BuildGetter(ISoundCloudRawClient soundCloudRawClient)
        {
            return async p => await soundCloudRawClient.Request<Track[]>(prefix, string.Empty, HttpMethod.Get, p);
        }
    }
}