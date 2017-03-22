﻿using System.Threading.Tasks;
using SoundCloud.API.Client.Objects;
using SoundCloud.API.Client.Objects.ExplorePieces;

namespace SoundCloud.API.Client.Subresources
{
    public interface IChartApi
    {
        Task<SCTrack[]> GetTracks(SCExploreCategory category, int offset = 0, int limit = 20);
    }
}
