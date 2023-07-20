using HackerNewsApi.Model;

namespace HackerNewsApi.Interface
{
    public interface IStoryService
    {
        Task<HackerNewsStoryResponse> GetStory(string storyId);
        Task<List<int>> GetStorys();

    }
}
