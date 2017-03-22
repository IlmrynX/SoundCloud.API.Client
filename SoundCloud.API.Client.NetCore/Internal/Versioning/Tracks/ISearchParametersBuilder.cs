using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Objects.Versioning;

namespace SoundCloud.API.Client.Internal.Versioning.Tracks
{
    internal interface ISearchParametersBuilder
    {
        Func<Dictionary<string, object>, Task<Track[]>> BuildGetter(SCApiVersion version, ISoundCloudRawClient soundCloudRawClient);
    }
}