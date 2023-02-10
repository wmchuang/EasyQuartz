// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace EasyQuartz.Monitoring;

public class JobDto
{
    public string Id { get; set; } = default!;

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