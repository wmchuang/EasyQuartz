// using EasyQuartz;
// using Microsoft.Extensions.Configuration;
// using Quartz;
// using System;
// using System.Threading.Tasks;
// using EasyQuartz.Storage;
//
// namespace ApiSample
// {
//     public class Test2Job : EasyQuartzJob
//     {
//         private readonly IConfiguration _configuration;
//
//         public Test2Job(IConfiguration configuration, IEasyQuartzJobStore easyQuartzJobStore) : base(easyQuartzJobStore)
//         {
//             _configuration = configuration;
//         }
//
//         public override string Cron => _configuration["Test2JobCron"];
//
//         public override Task Execute()
//         {
//             Console.WriteLine($"{DateTime.Now}我是  Test2Job");
//             return Task.CompletedTask;
//         }
//     }
// }
