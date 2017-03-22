using System.IO;
using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;
using SoundCloud.API.Client.Objects.TrackPieces;
using SoundCloud.API.Client.Objects.Versioning;
using SoundCloud.API.Client.Subresources.Helpers;

namespace SoundCloud.API.Client.Subresources
{
    public interface ITracksApi
    {
        Task<SCTrack> UploadTrack(Stream trackFileStream, string title, string description, SCSharing sharing, Stream artworkFileStream = null);
        ITracksSearcher BeginSearch(SCFilter filter, SCApiVersion version = SCApiVersion.V2);
    }
}