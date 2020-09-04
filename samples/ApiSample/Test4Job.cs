using EasyQuartz;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ApiSample
{
    [JobIgnore]
    public class Test4Job : IJob
    {
        private readonly IJobManager _jobManager;

        public Test4Job(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            string id = dataMap.GetString("Id");

            Console.WriteLine($"{DateTime.Now}Test4Job,参数{id}");
            //await _jobManager.RemoveJobAsync(typeof(Test4Job), id);
            return Task.CompletedTask;
        }
    }
}
