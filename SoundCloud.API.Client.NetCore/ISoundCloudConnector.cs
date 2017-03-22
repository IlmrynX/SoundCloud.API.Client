using System;
using System.Threading.Tasks;
using SoundCloud.API.Client.Objects.Auth;

namespace SoundCloud.API.Client
{
    public interface ISoundCloudConnector
    {
        IAnonymousSoundCloudClient AnonymousConnect();
        IUnauthorizedSoundCloudClient UnauthorizedConnect(string clientId, string clientSecret);
        Task<ISoundCloudClient> DirectConnect(string clientId, string clientSecret, string userName, string password);
        Task<ISoundCloudClient> Connect(string clientId, string clientSecret, string code, string redirectUri);
        ISoundCloudClient Connect(SCAccessToken accessToken);
        Uri GetRequestTokenUri(string clientId, string redirectUri, SCResponseType responseType, SCScope scope, SCDisplay display, string state);
        Task<SCAccessToken> RefreshToken(string clientId, string clientSecret, string token);
    }
}