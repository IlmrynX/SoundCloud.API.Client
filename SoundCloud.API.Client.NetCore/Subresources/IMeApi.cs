using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;
using SoundCloud.API.Client.Objects.ConnectionPieces;

namespace SoundCloud.API.Client.Subresources
{
    public interface IMeApi : IUserApi
    {
        Task<SCConnection[]> GetConnections(int offset = 0, int limit = 50);
        Task<string> PostConnection(SCServiceType serviceType, string redirectUri);

        Task<SCActivityResult> GetActivityQueryResult(string queryId);
        Task<SCActivityResult> GetRecentActivities(string cursorToNext = null);
        Task<SCActivityResult> GetRecentAllActivities(string cursorToNext = null);
        Task<SCActivityResult> GetRecentFollowingTracks(string cursorToNext = null);
        Task<SCActivityResult> GetRecentExclusivelySharedTracks(string cursorToNext = null);
        Task<SCActivityResult> GetRecentUserTracksActivities(string cursorToNext = null);
    }
}