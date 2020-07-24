using EasyQuartz;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ApiSample
{
    [JobIgnore]
    public class Test4Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test4Job");
            return Task.CompletedTask;
        }
    }
}
