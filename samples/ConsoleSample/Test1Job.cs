using EasyQuartz;
using Quartz;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ConsoleSample
{
    [TriggerCron("0/10 * * * * ? *")]
    [Description("我是一个测试Job")]
    public class Test1Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now} Test1Job");
            return Task.CompletedTask;
        }
    }
}
