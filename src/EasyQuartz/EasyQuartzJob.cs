using System.Threading.Tasks;
using EasyQuartz.Storage;
using Quartz;

namespace EasyQuartz
{
    public abstract class EasyQuartzJob : IJob
    {
        private readonly IEasyQuartzJobStore _easyQuartzJobStore;

        protected EasyQuartzJob(IEasyQuartzJobStore easyQuartzJobStore)
        {
            _easyQuartzJobStore = easyQuartzJobStore;
        }

        public abstract string Cron { get; }

        public Task Execute(IJobExecutionContext context)
        {
            // _easyQuartzJobStore.SaveAsync(new JobLog());
            Execute();
            return Task.CompletedTask;
        }

        public abstract Task Execute();
    }
}