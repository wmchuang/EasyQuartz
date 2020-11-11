//using EasyQuartz;
//using Quartz;
//using System;
//using System.Threading.Tasks;

//namespace ApiSample
//{
//    [StartNow]
//    [TriggerCron("0 0/5 * * * ? ")]
//    public class Test3Job : IJob
//    {
//        private readonly IJobManager _jobManager;

//        public Test3Job(IJobManager jobManager)
//        {
//            _jobManager = jobManager;
//        }

//        public async Task Execute(IJobExecutionContext context)
//        {
//            Console.WriteLine($"{DateTime.Now}我是  Test3Job");
//            await _jobManager.AddJobAsync(typeof(Test4Job), CronCommon.SecondInterval(2), "111111");
//        }
//    }
//}
