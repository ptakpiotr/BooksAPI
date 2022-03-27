using AutoMapper;
using Hangfire;

namespace WebAPI.Jobs
{
    public class RecurringJobs : IRecurringJobs
    {
        private readonly IRecurringJobManager _manager;
        private readonly IBookRepo _repo;
        private readonly IRefitClient _cli;
        private readonly string _apiKey;

        public RecurringJobs(IRecurringJobManager manager, IConfiguration configuration, IBookRepo repo, IRefitClient cli)
        {
            _manager = manager;
            _repo = repo;
            _cli = cli;
            _apiKey = configuration["ApiKey"];
        }

        public void PerformJob()
        {
            _manager.AddOrUpdate("CallApi", () => CallApi(), Cron.Daily);
        }

        public async Task CallApi()
        {
            var data = await _cli.GetBooks(_apiKey, 10);
            _repo.AddMultipleBook(data);
            _repo.SaveChanges();
        }
    }
}
