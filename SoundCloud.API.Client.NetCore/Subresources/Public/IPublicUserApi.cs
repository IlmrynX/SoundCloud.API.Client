using System;
using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources.Public
{
    public interface IPublicUserApi
    {
        Task<SCUser> GetUser();
        Task<SCTrack[]> GetTracks(int offset = 0, int limit = 50);
        Task<SCPlaylist[]> GetPlaylists(int offset = 0, int limit = 50);

        Task<SCUser[]> GetFollowings(int offset = 0, int limit = 50);

        [Obsolete("API BUG. Use GetFollowings(). This method returns 401. It's API trouble. More here: https://github.com/soundcloud/soundcloud-ruby/issues/24")]
        Task<SCUser> GetFollowing(string followingUserId);

        Task<SCUser[]> GetFollowers(int offset = 0, int limit = 50);
        
        [Obsolete("API BUG. Use GetFollowers(). This method returns 401. It's API trouble. More here: https://github.com/soundcloud/soundcloud-ruby/issues/24")]
        Task<SCUser> GetFollower(string followerUserId);

        Task<SCComment[]> GetComments(int offset = 0, int limit = 50);

        Task<SCTrack[]> GetFavorites(int offset = 0, int limit = 50);
        Task<SCTrack> GetFavorite(string favoriteTrackId);

        Task<SCGroup[]> GetGroups(int offset = 0, int limit = 50);

        Task<SCWebProfile[]> GetWebProfiles(int offset = 0, int limit = 50); 
    }
}