using System;
using System.Threading;
using System.Threading.Tasks;
using EasyQuartz.Storage;
using EasyQuartz.Storage.Model;
using Quartz;

namespace EasyQuartz.Listener;

public class CustomSchedulerListener : ISchedulerListener
{
    private readonly IEasyQuartzJobStore _easyQuartzJobStore;

    public CustomSchedulerListener(IEasyQuartzJobStore easyQuartzJobStore)
    {
        _easyQuartzJobStore = easyQuartzJobStore;
    }

    public async Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default)
    {
        var job = new Job
        {
            JobKey = jobDetail.Key.ToString(),
            JobDesc = jobDetail.Description
        };
        if (job.JobKey.EndsWith("_Now")) return;

        await _easyQuartzJobStore.AddJobAsync(job);
    }

    public async Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => { Console.WriteLine($"{jobKey} 被删除 "); });
    }

    public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(trigger.Description))
        {
            await _easyQuartzJobStore.SetJobCronAsync(trigger.Description, trigger.JobKey.ToString());
        }
    }

    public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SchedulerShutdown(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SchedulerShuttingdown(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task SchedulerStarted(CancellationToken cancellationToken = default)
    {
        // await Task.Run(() => { Console.WriteLine($"this is SchedulerStarted"); });
    }

    public async Task SchedulerStarting(CancellationToken cancellationToken = default)
    {
        // await Task.Run(() => { Console.WriteLine($"this is SchedulerStarting"); });
    }

    public Task SchedulingDataCleared(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}