using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YouTubeOnDemandStats.Models;
using YouTubeOnDemandStats.Services;
using System.Linq;
using System.Collections.Generic;

namespace YouTubeOnDemandStats
{
	public static class YouTubeStatsFunction
    {
        [FunctionName("GetChannelVideoStats")]
        public static async Task<IActionResult> GetChannelVideoStats(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetChannelVideoStats/{ids}")] HttpRequest req,
            string ids,
            ILogger log)
        {

            var apiKey = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
            var appName = Environment.GetEnvironmentVariable("YOUTUBE_APP_NAME");

            var service = new YouTubeStatsService(apiKey, appName);

            var stats = new List<Stats>();

            foreach (var channel in ids.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var t = await service.GetVideosOnChannel(channel);
                var t2 = t.Select(v => new Stats
                {
                    Id = v.Id.Insert(0, "k"),
                    ChannelId = channel,
                    ChannelTitle = v.Snippet?.ChannelTitle ?? string.Empty,
                    Date = v.Snippet.PublishedAt ?? DateTime.Now,
                    MonthYear = (v.Snippet?.PublishedAt ?? DateTime.Now).ToString("MMM-yy"),
                    ViewCount = v.Statistics?.ViewCount ?? 0,
                    CommentCount = v.Statistics?.CommentCount ?? 0,
                    DislikeCount = v.Statistics?.DislikeCount ?? 0,
                    FavoriteCount = v.Statistics?.FavoriteCount ?? 0,
                    LikeCount = v.Statistics?.LikeCount ?? 0,
                    Topic = v.Snippet?.Title ?? string.Empty
                }).ToList();
                stats.AddRange(t2);
            }

            return new OkObjectResult(stats);
        }
    }
}
