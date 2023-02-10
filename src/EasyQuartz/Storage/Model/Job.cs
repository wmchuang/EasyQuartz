using System;

namespace EasyQuartz.Storage.Model;

/// <summary>
/// 任务
/// </summary>
public class Job
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Id { get; set; } = SnowflakeId.Default().NextId().ToString();

    /// <summary>
    /// 任务名称
    /// </summary>
    public string JobKey { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string JobDesc { get; set; }

    /// <summary>
    /// Cron
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// 最后一次执行时间
    /// </summary>
    public DateTime? LastFireTime { get; set; }

    /// <summary>
    /// 下一次执行时间
    /// </summary>
    public DateTime? NextFireTime { get; set; }
}