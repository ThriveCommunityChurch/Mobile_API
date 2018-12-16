using System.Collections.Generic;
using System.Threading.Tasks;
using ThriveChurchOfficialAPI.Core;

namespace ThriveChurchOfficialAPI.Repositories
{
    public interface ISermonsRepository
    {
        Task<AllSermonsResponse> GetAllSermons();

        Task<SermonSeries> CreateNewSermonSeries(SermonSeries request);

        Task<SermonSeries> GetSermonSeriesForId(string SeriesId);

        Task<LiveSermons> GetLiveSermons();

        Task<LiveSermons> UpdateLiveSermons(LiveSermons request);

        Task<LiveSermons> UpdateLiveSermonsInactive();

        Task<SermonSeries> UpdateSermonSeries(SermonSeries request);

        Task<SermonSeries> GetSermonSeriesForSlug(string slug);
    }
}