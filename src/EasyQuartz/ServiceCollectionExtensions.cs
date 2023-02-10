using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EasyQuartz.Options;
using EasyQuartz.Storage;

namespace EasyQuartz
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEasyQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            #region Add JobSchedule

            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob)) && t.IsClass && !t.IsAbstract))
                .ToArray();
            foreach (var jobType in jobTypes)
            {
                services.AddTransient(jobType);
                var ignore = jobType.GetTypeInfo().IsDefined(typeof(JobIgnoreAttribute), false);
                var startNow = jobType.GetTypeInfo().IsDefined(typeof(StartNowAttribute), false);
                if (ignore)
                    continue;

                string cron;
                if (jobType.BaseType == typeof(EasyQuartzJob))
                {
                    var jobService = services.BuildServiceProvider().GetService(jobType);
                    cron = ((EasyQuartzJob)jobService).Cron;
                }
                else
                {
                    var triggerCron = jobType.GetCustomAttributes().OfType<TriggerCronAttribute>().FirstOrDefault();
                    if (triggerCron == null)
                        continue;

                    cron = triggerCron.Cron;
                }

                if (string.IsNullOrWhiteSpace(cron)) continue;
                
                var name = jobType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>()?.Description ?? jobType.Name;
                var schedule = new JobSchedule(jobType, cron, name, $"{jobType.Name}Group", startNow);
                services.AddSingleton(schedule);
            }

            #endregion

            services.AddHostedService<QuartzHostedService>();
            services.AddTransient<IJobManager, JobManager>();
        }

        public static void AddEasyQuartz(this IServiceCollection services, Action<EasyQuartzOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new EasyQuartzOptions();
            setupAction(options);
            
            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }

            services.Configure(setupAction);

            // 如果不用库存储，获取不到初始化的服务，则不用执行引导程序
            using var provider = services.BuildServiceProvider();
            var storageInitializer = provider.GetService<IEasyQuartzJobStore>();
            if (storageInitializer != null)
                services.AddHostedService<Bootstrapper>();

            AddEasyQuartz(services);
        }
    }
}