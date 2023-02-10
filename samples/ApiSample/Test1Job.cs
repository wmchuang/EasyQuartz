using EasyQuartz;
using Quartz;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ApiSample
{
    [TriggerCron("0/30 * * * * ? *")]
    [Description("订单统计任务")]
    [StartNow]
    public class Test1Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test1Job");
            return Task.CompletedTask;
        }
    }
}
