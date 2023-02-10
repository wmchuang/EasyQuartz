using System;
using System.Collections.Generic;

namespace EasyQuartz.Options;

public class EasyQuartzOptions
{
    /// <summary>
    /// 记录保存时长,默认1天
    /// </summary>
    public TimeSpan RetainTime { get; set; } = TimeSpan.FromDays(1);

    /// <summary>
    /// 扩展子服务
    /// </summary>
    internal IList<IEasyQuartzOptionsExtension> Extensions { get; } = new List<IEasyQuartzOptionsExtension>(0);

    /// <summary>
    /// Registers an extension that will be executed when building services.
    /// </summary>
    /// <param name="extension"></param>
    public void RegisterExtension(IEasyQuartzOptionsExtension extension)
    {
        if (extension == null)
        {
            throw new ArgumentNullException(nameof(extension));
        }

        Extensions.Add(extension);
    }
}