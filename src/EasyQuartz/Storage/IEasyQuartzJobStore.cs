using System;
using System.Threading;
using System.Threading.Tasks;
using EasyQuartz.Monitoring;
using EasyQuartz.Storage.Model;

namespace EasyQuartz.Storage;

public interface IEasyQuartzJobStore
{
    //dashboard api
    IMonitoringApi GetMonitoringApi();

    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    Task AddJobAsync(Job job);

    /// <summary>
    /// 设置任务Cron
    /// </summary>
    /// <param name="cron"></param>
    /// <param name="jobKey"></param>
    /// <returns></returns>
    Task SetJobCronAsync(string cron, string jobKey);

    /// <summary>
    /// 设置任务执行时间信息
    /// </summary>
    /// <param name="fireTime"></param>
    /// <param name="nextFireTime"></param>
    /// <param name="jobKey"></param>
    /// <returns></returns>
    Task SetJobFireAsync(string fireTime, string nextFireTime, string jobKey);

    /// <summary>
    /// 保存任务记录记录
    /// </summary>
    /// <param name="jobLog"></param>
    /// <returns></returns>
    Task SaveLogAsync(JobLog jobLog);

    /// <summary>
    /// 删除过期记录
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="batchCount"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<int> DeleteExpiresAsync(DateTime timeout, int batchCount = 1000, CancellationToken token = default);
}