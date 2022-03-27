
namespace WebAPI.Jobs
{
    public interface IRecurringJobs
    {
        Task CallApi();
        void PerformJob();
    }
}