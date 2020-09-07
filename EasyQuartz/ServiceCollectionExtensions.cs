using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;

namespace EasyQuartz
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEasyQuartzService(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            #region Add JobSchedule

            JobSchedule schedule = null;

            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                           .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob)) && t.IsClass && !t.IsAbstract))
                           .ToArray();
            foreach (var jobType in jobTypes)
            {
                services.AddTransient(jobType);
                var attribute = jobType.GetCustomAttributes(typeof(JobIgnoreAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    continue;
                }
                string cron = string.Empty;
                if (jobType.BaseType == typeof(EasyQuartzJob))
                {
                    var jobService = services.BuildServiceProvider().GetService(jobType);
                    cron = ((EasyQuartzJob)jobService).Cron;
                }
                else
                {
                    attribute = jobType.GetCustomAttributes(typeof(TriggerCronAttribute), false).FirstOrDefault();
                    if (attribute == null)
                    {
                        continue;
                    }
                    cron = ((TriggerCronAttribute)attribute).Cron;
                }
                if (!string.IsNullOrWhiteSpace(cron))
                {
                    schedule = new JobSchedule(jobType, cron, $"{jobType.Name}Group");
                    services.AddSingleton(schedule);
                }
            }

            #endregion

            services.AddHostedService<QuartzHostedService>();
            services.AddTransient<IJobManager, JobManager>();
        }
    }
}
