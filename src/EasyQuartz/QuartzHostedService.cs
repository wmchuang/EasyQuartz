﻿using System;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyQuartz.Listener;
using EasyQuartz.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EasyQuartz
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private readonly IJobFactory _jobFactory;
        private readonly IServiceProvider _serviceProvider;

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IEnumerable<JobSchedule> jobSchedules,
            IJobFactory jobFactory,
            IServiceProvider serviceProvider)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
            _serviceProvider = serviceProvider;
        }

        private IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            System.Console.WriteLine("QuartzHostedService Run");
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            var easyQuartzJobStore = _serviceProvider.GetService<IEasyQuartzJobStore>();

            //添加监听
            if (easyQuartzJobStore != null)
            {
                Scheduler.ListenerManager.AddJobListener(new CustomJobListener(easyQuartzJobStore));
                // Scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener());
                Scheduler.ListenerManager.AddSchedulerListener(new CustomSchedulerListener(easyQuartzJobStore));
            }

            foreach (var jobSchedule in _jobSchedules)
            {
                if (jobSchedule.StartNow)
                {
                    var nowJob = CreateJob(jobSchedule, "_Now");
                    var nowTrigger = CreateNowTrigger();
                    await Scheduler.ScheduleJob(nowJob, nowTrigger, cancellationToken);
                }

                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // ReSharper disable once PossibleNullReferenceException
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(JobSchedule schedule, string now = "")
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity($"{jobType.FullName}{now}", schedule.Group)
                .WithDescription(schedule.JobDesc)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger", schedule.Group)
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }

        private static ITrigger CreateNowTrigger()
        {
            return TriggerBuilder
                .Create()
                .StartNow()
                .Build();
        }
    }
}