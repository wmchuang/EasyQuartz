using EasyQuartz;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ApiSample
{
    public class Test2Job : EasyQuartzJob, IJob
    {
        private readonly IConfiguration _configuration;

        public Test2Job(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override string Cron => _configuration["Test2JobCron"];

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test2Job");
            return Task.CompletedTask;
        }
    }
}
