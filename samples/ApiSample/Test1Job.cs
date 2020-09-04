//using EasyQuartz;
//using Quartz;
//using System;
//using System.Threading.Tasks;

//namespace ApiSample
//{
//    [TriggerCron("0/1 * * * * ? *")]
//    public class Test1Job : IJob
//    {
//        public Task Execute(IJobExecutionContext context)
//        {
//            Console.WriteLine($"{DateTime.Now}我是  Test1Job");
//            return Task.CompletedTask;
//        }
//    }
//}
