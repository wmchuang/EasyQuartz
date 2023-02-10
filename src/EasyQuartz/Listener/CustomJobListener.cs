using System;
using System.Threading;
using System.Threading.Tasks;
using EasyQuartz.Storage;
using EasyQuartz.Storage.Model;
using Quartz;

namespace EasyQuartz.Listener;

public class CustomJobListener : IJobListener
{
    private readonly IEasyQuartzJobStore _easyQuartzJobStore;

    public CustomJobListener(IEasyQuartzJobStore easyQuartzJobStore)
    {
        _easyQuartzJobStore = easyQuartzJobStore;
    }

    public string Name => "CustomJobListener";

    public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        // await Task.Run(() =>
        // {
        //     //便于我们自己添加自己的业务逻辑
        //     Console.WriteLine($"{DateTime.Now} this is JobExecutionVetoed");
        // });
    }

    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        // await Task.Run(() => { Console.WriteLine($"{DateTime.Now} this is JobToBeExecuted"); });
    }

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
    {
        var log = new JobLog
        {
            JobKey = context.JobDetail.Key.ToString(),
            FireTime = context.FireTimeUtc.ToLocalTime().DateTime,
            RunTime = Convert.ToInt32(context.JobRunTime.TotalMilliseconds),
        };
        await _easyQuartzJobStore.SaveLogAsync(log);

        var fireTime =  context.FireTimeUtc.ToLocalTime().DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        var nextFireTime = context.NextFireTimeUtc?.ToLocalTime().DateTime.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
        await _easyQuartzJobStore.SetJobFireAsync(fireTime, nextFireTime, context.JobDetail.Key.ToString());
    }
}