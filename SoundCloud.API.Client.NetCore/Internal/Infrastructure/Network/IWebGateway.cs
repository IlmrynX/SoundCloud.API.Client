using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client.Helpers;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using File = SoundCloud.API.Client.Internal.Infrastructure.Objects.Uploading.File;

namespace SoundCloud.API.Client.Internal.Infrastructure.Network
{
    internal interface IWebGateway
    {
        Task<string> Request(IUriBuilder uriBuilder, HttpMethod method, Dictionary<string, object> parameters, byte[] body, string accessToken);
        Task<Stream> RequestStream(IUriBuilder uriBuilder, HttpMethod method, Dictionary<string, object> parameters, byte[] body, string accessToken);
        Task<string> Upload(IUriBuilder uriBuilder, Dictionary<string, object> parameters, params File[] files);
    }
}