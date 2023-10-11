using HackerNewsApi.Interface;
using HackerNewsApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BestStoriesController : ControllerBase
    {
        private IStoryService _storyService;


        public BestStoriesController(IStoryService storyService)
        {
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
                foreach (var storyId in apiResponse.Take(numberOfStories))
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
