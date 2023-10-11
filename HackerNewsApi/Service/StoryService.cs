using HackerNewsApi.Contract;
using HackerNewsApi.Interface;
using HackerNewsApi.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace HackerNewsApi.Service
{
    public class StoryService : IStoryService
    {
        private static readonly Object _lock = new Object();

        private readonly ApiSource _apiSource;
        private IGenericApiService _genericApiService;
        private IDistributedCache _distributedCache;


        public StoryService(IOptions<ApiSource> apiSource,
            IGenericApiService genericApiService,
            IDistributedCache distributedCache)
        {
            _apiSource = apiSource.Value;
            _genericApiService = genericApiService;
            _distributedCache = distributedCache;
        }
        public async Task<HackerNewsStoryResponse> GetStory(string storyId)
        {
            HackerNewsStory? hackerNewsStory = GetFromCache(storyId);

            if (hackerNewsStory != null)
            {
                return ReturnResponse(hackerNewsStory);
            }

            var apiResponse = await _genericApiService.GetAsync<HackerNewsStory>(
                new Uri(_apiSource.HackerNewApiUrl + $"item/{storyId}.json")).ConfigureAwait(false);
            if (apiResponse != null)
            {
                AddToCache(storyId, apiResponse);
                return ReturnResponse(apiResponse);
            }

            return new HackerNewsStoryResponse { title = $"Story Id {storyId} Not found", commentCount = 0 };

        }

        public async Task<List<int>> GetStorys()
        {
            var apiResponse = await _genericApiService.GetAsync<List<int>>(
                new Uri(_apiSource.HackerNewApiUrl + $"beststories.json")).ConfigureAwait(false);

            return apiResponse;

        }

        private HackerNewsStoryResponse ReturnResponse(HackerNewsStory hackerNewsStory)
        {
            return new HackerNewsStoryResponse
            {
                commentCount = hackerNewsStory.kids.Count(),
                postedBy = hackerNewsStory.by,
                score = hackerNewsStory.score,
                time = DateTimeOffset.FromUnixTimeSeconds(hackerNewsStory.time).DateTime.ToString("yyyy-MM-ddTHH:MM:ss"),
                title = hackerNewsStory.title,
                url = hackerNewsStory.url
            };
        }

        private HackerNewsStory? GetFromCache(string storyId)
        {
            var storyItem = _distributedCache.GetString(storyId);
            if (storyItem != null)
            {
                return JsonConvert.DeserializeObject<HackerNewsStory>(storyItem);
            }
            return null;
        }

        private void AddToCache(string storyId, HackerNewsStory hackerNewsStory)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(_apiSource.CacheExpirationInDays));

            lock (_lock)
            {
                _distributedCache.SetString(storyId, JsonConvert.SerializeObject(hackerNewsStory), options);
            }
        }
    }
}
