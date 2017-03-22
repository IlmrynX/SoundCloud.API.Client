using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources
{
    public interface IResolveApi
    {
        Task<SCUser> GetUser(string url);
        Task<SCTrack> GetTrack(string url);
        Task<SCPlaylist> GetPlaylist(string url);
        Task<SCGroup> GetGroup(string url);
    }
}