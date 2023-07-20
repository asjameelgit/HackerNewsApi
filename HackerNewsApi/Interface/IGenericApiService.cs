namespace HackerNewsApi.Interface
{
    public interface IGenericApiService
    {
        Task<T> GetAsync<T>(Uri requestUrl);

    }
}
