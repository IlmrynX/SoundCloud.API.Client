﻿using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Subresources
{
    public interface IPlaylistApi
    {
        //note: GetPlaylists doesn't support. Returns 500. You can try it here: https://developers.soundcloud.com/console
        Task<SCPlaylist> GetPlaylist();

        //todo:
        //GET, POST, PUT, DELETE	/playlists/{id}/shared-to/users	users who have access to the track
        //GET, POST, PUT, DELETE	/playlists/{id}/shared-to/emails	email addresses who are invited to the playlist
        //GET, PUT	/playlists/{id}/secret-token	secret token of the playlist
    }
}