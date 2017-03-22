using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources.Public
{
    public interface IPublicGroupApi
    {
        Task<SCGroup> GetGroup();

        Task<SCUser[]> GetModerators(int offset = 0, int limit = 50);
        Task<SCUser[]> GetMembers(int offset = 0, int limit = 50);
        Task<SCUser[]> GetContributors(int offset = 0, int limit = 50);
        Task<SCUser[]> GetUsers(int offset = 0, int limit = 50);

        Task<SCTrack[]> GetApprovedTracks(int offset = 0, int limit = 50);
    }
}