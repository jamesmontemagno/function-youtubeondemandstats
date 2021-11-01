using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeOnDemandStats.Services
{
    public class YouTubeStatsService
    {
        readonly YouTubeService youTubeService;

        public YouTubeStatsService(string apiKey, string appName)
        {
            youTubeService =
                new YouTubeService(
                    new BaseClientService.Initializer()
                    {
                        ApiKey = apiKey,
                        ApplicationName = appName
                    });
        }

        public async Task<IList<Video>> GetVideosOnChannel(string channelId)
        {
            var searchResults = new List<SearchResult>();
            var response = new SearchListResponse();
            var request = youTubeService.Search.List("snippet,id");
            request.ChannelId = channelId;
            request.MaxResults = 100;
            do
            {
                request.PageToken = response.NextPageToken;
                response = await request.ExecuteAsync();
                searchResults.AddRange(response.Items);
            } while (response.NextPageToken != null);

            return await GetVideosAsync(searchResults.Select(p => p.Id.VideoId).ToArray(), null);
        }

        public async Task<IList<Video>> GetVideosInPlaylist(string playlistId)
        {
            var playlist = new List<PlaylistItem>();
            var response = new PlaylistItemListResponse();
            var request = youTubeService.PlaylistItems.List("snippet,status");
            request.PlaylistId = playlistId;
            request.MaxResults = 100;
            do
            {
                request.PageToken = response.NextPageToken;
                response = await request.ExecuteAsync();
                playlist.AddRange(response.Items);
            } while (response.NextPageToken != null);

            return await GetVideosAsync(playlist.Select(p => p.Snippet.ResourceId.VideoId).ToArray(), null);
        }

        public async Task<IList<Video>> GetVideosAsync(string[] videoIds, List<Video> videos)
        {
            var request = youTubeService.Videos.List(new[] { "snippet", "statistics" });
            var response = new VideoListResponse();

            videos = videos ?? new List<Video>();

            var idsToGet = videoIds;
            if (idsToGet.Length > 50)
            {
                idsToGet = videoIds.Take(50).ToArray();
                videoIds = videoIds.Skip(50).ToArray();
            }
            else
            {
                videoIds = new string[] { };
            }
            request.Id = idsToGet;
            //request.MaxResults = videoIds.Length;
            do
            {
                request.PageToken = response.NextPageToken;
                response = await request.ExecuteAsync();
                videos.AddRange(response.Items);
            } while (response.NextPageToken != null);



            return videoIds.Length > 0 ? await GetVideosAsync(videoIds, videos) : videos;
        }
    }
}
