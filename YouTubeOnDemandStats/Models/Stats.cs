using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeOnDemandStats.Models
{
    public class Stats
    {
        public string Id { get; set; }
        public string Show { get; set; }
        public string ShowId { get; set; }

        public string ChannelTitle { get; set; }
        public string ChannelId { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public string MonthYear { get; set; }
        public DateTime Date { get; set; }
        public ulong ViewCount { get; set; }
        public ulong FavoriteCount { get; set; }
        public ulong DislikeCount { get; set; }
        public ulong LikeCount { get; set; }
        public ulong CommentCount { get; set; }

    }
}
