using System;

namespace EasyQuartz.Monitoring;

public class LogDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string JobKey { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime FireTime { get; set; }

    /// <summary>
    /// 运行时间（毫秒）
    /// </summary>
    public int RunTime { get; set; }
}