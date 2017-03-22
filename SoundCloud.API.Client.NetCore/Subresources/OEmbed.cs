using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Client.Helpers;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Objects;
using SoundCloud.API.Client.Subresources.Helpers;

namespace SoundCloud.API.Client.Subresources
{
    public class OEmbed : IOEmbed
    {
        private readonly ISoundCloudRawClient soundCloudRawClient;

        private const string prefix = "oembed";

        internal OEmbed(ISoundCloudRawClient soundCloudRawClient)
        {
            this.soundCloudRawClient = soundCloudRawClient;
        }

        public IOEmbedQuery BeginQuery(string url)
        {
            return new OEmbedQuery(url, 
                                   async p => await soundCloudRawClient.Request<SCOEmbed>(prefix, string.Empty, HttpMethod.Get, p,
                        responseType: null, domain: Domain.Direct),
                                   async p => await soundCloudRawClient.Request<string>(prefix, string.Empty, HttpMethod.Get, p, responseType: null, domain: Domain.Direct));
        }
    }
}