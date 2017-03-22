using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources
{
    public interface IGroupApi
    {
        Task<SCGroup> GetGroup();

        Task<SCUser[]> GetModerators(int offset = 0, int limit = 50);
        Task<SCUser[]> GetMembers(int offset = 0, int limit = 50);
        Task<SCUser[]> GetContributors(int offset = 0, int limit = 50);
        Task<SCUser[]> GetUsers(int offset = 0, int limit = 50);

        Task<SCTrack[]> GetApprovedTracks(int offset = 0, int limit = 50);
        Task<SCTrack[]> GetPendingTracks(int offset = 0, int limit = 50);
        Task<SCTrack> GetPendingTrack(string trackId);
        Task AcceptPendingTrack(string trackId);
        Task RejectPendingTrack(string trackId);

        Task<SCTrack[]> GetContributions(int offset = 0, int limit = 50);
        Task<SCTrack> GetContribution(string trackId);
        Task CreateContribution(string trackId);
        Task DeleteContribution(string trackId);
    }
}