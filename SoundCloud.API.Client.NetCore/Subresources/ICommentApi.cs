using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources
{
    public interface ICommentApi
    {
        Task<SCComment> GetComment();
    }
}