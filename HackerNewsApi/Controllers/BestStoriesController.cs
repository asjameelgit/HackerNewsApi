using HackerNewsApi.Contract;
using HackerNewsApi.Interface;
using HackerNewsApi.Model;
using HackerNewsApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace HackerNewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly ApiSource _apiSource;
        private IGenericApiService _genericApiService;
        private IStoryService _storyService;


        public BestStoriesController(IOptions<ApiSource> apiSource,
            IStoryService storyService)
        {
            _apiSource = apiSource.Value;
            _storyService = storyService;
        }

        [HttpGet(Name = "GetBestStories")]
        public async Task<IActionResult> Get(int numberOfStories)
        {
            if (numberOfStories <= 0)
                return StatusCode(StatusCodes.Status400BadRequest, "Number of stories should not be less than or equal to 0");

            var apiResponse = await _storyService.GetStorys();

            if (apiResponse != null && apiResponse.Count > 0)
            {
                var returnList = new List<HackerNewsStoryResponse>();
                foreach(var storyId in apiResponse.Take(numberOfStories))
                {
                    var storyItem = await _storyService.GetStory(storyId.ToString());
                    returnList.Add(storyItem);
                }

                return Ok(returnList.OrderByDescending(x => x.commentCount).ToList());
            }
            else
            {
                return Ok("No Stories found");
            }
        }

    }
}
