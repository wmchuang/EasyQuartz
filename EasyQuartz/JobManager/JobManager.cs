using Quartz;
using Quartz.Spi;
using System;
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

        public async Task AddJobAsync(Type jobType, string cron)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            IJobDetail job = JobBuilder.Create(jobType).WithIdentity(jobType.FullName, $"{jobType.FullName}Group")
                .WithDescription(jobType.Name)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"{jobType.FullName}.trigger", $"{jobType.FullName}Group")
                .WithCronSchedule(cron)
                .WithDescription(jobType.Name)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
