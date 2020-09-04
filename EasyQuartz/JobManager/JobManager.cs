using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyQuartz
{
    public class JobManager : IJobManager
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;

        public JobManager(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task AddJobAsync(Type jobType, string cron, string mark = "")
        {
            var name = jobType.FullName + mark;

            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
            if (jobKeys.Count > 0)
            {
                if (jobKeys.Any(x => x.Name == name))
                {
                    return;
                }
            }
            IJobDetail job = JobBuilder.Create(jobType).WithIdentity(name, $"{name}Group")
            .WithDescription(jobType.Name)
            .UsingJobData("Id", mark)
            .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"{name}.trigger", $"{name}Group")
                .WithCronSchedule(cron)
                .WithDescription(jobType.Name)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        public async Task RemoveJobAsync(Type jobType, string mark = "")
        {
            var name = jobType.FullName + mark;
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
            await scheduler.DeleteJobs(jobKeys.Where(x => x.Name == name).ToList());
        }
    }
}
