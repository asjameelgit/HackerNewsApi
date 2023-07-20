using System;

namespace HackerNewsApi.Model
{
    public class HackerNewsStoryResponse
    {
        public string postedBy { get; set; }
        public int commentCount { get; set; }
        public int score { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
