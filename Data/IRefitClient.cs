using Refit;

namespace WebAPI.Data
{
//https://api.mockaroo.com/api/generate.json?key=2a384b20&schema=Books&count=10
    public interface IRefitClient
    {
        [Get("/api/generate.json?key={key}&schema=Books&count={max}")]
        Task<List<BookAddDTO>> GetBooks(string key,int max);
    }
}
