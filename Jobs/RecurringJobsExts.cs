namespace WebAPI.Jobs
{
    public static class RecurringJobsExts
    {
        public static void UseRecurringJobs(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                IRecurringJobs jobs = scope.ServiceProvider.GetService<IRecurringJobs>();
                PerformRecurringJobs(jobs);
            }
        }

        private static void PerformRecurringJobs(IRecurringJobs jobs)
        {
            jobs.PerformJob();
        }
    }
}
