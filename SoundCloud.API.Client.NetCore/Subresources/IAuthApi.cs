using System;
using System.Threading.Tasks;
using SoundCloud.API.Client.Objects.Auth;

namespace SoundCloud.API.Client.Subresources
{
    public interface IAuthApi
    {
        SCAccessToken CurrentToken { get; }
        Task<SCAccessToken> AuthorizeByPassword(string userName, string password);
        Task<SCAccessToken> AuthorizeByCode(string code, string redirectUri);
        Uri GetRequestTokenUri(string redirectUri, SCResponseType responseType, SCScope scope, SCDisplay display, string state);
        Task<SCAccessToken> RefreshToken(string token);
    }
}