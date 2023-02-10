using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;

namespace EasyQuartz
{
    public class JobManager : IJobManager
    {
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobManager(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task AddJobAsync(Type jobType, string cron, string id = "")
        {
            var name = jobType.FullName + id;

            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
            if (jobKeys.Count > 0)
                if (jobKeys.Any(x => x.Name == name))
                    return;

            var job = JobBuilder.Create(jobType).WithIdentity(name, $"{name}Group")
                .WithDescription(jobType.Name)
                .UsingJobData("Id", id)
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

        public async Task RemoveJobAsync(Type jobType, string id = "")
        {
            var name = jobType.FullName + id;
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
            await scheduler.DeleteJobs(jobKeys.Where(x => x.Name == name).ToList());
        }

        public async Task PauseJob(Type jobType, string id = "")
        {
            var name = jobType.FullName + id;
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
            await scheduler.PauseJob(jobKeys.First(x => x.Name == name));
        }

        public async Task OperateJob(Type jobType, OperateEnum operate, string id = "")
        {
            var name = jobType.FullName + id;
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
            var key = jobKeys.FirstOrDefault(x => x.Name == name);

            if (key == null) return;
            switch (operate)
            {
                case OperateEnum.Delete:
                    await scheduler.DeleteJob(key);
                    break;
                case OperateEnum.Pause:
                    await scheduler.PauseJob(key);
                    break;
                case OperateEnum.Resume:
                    await scheduler.ResumeJob(key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operate), operate, null);
            }
        }

        public async Task RunJobAsync(string jobKey)
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();

                var key = jobKey.Substring(jobKey.IndexOf('.') + 1);
                var job = new JobKey(key,jobKey.Split('.')[0]);
                await scheduler.TriggerJob(job);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
        }
    }
}