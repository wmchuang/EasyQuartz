using EasyQuartz.Monitoring;
using EasyQuartz.Options;
using EasyQuartz.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyQuartzStore;

public class MySqlAuditingOptionsExtension : IEasyQuartzOptionsExtension
{
    private readonly Action<MySqlOptions> _configure;

    public MySqlAuditingOptionsExtension(Action<MySqlOptions> configure)
    {
        _configure = configure;
    }

    /// <summary>
    /// 添加服务
    /// </summary>
    /// <param name="services"></param>
    public void AddServices(IServiceCollection services)
    {
        services.TryAddSingleton<IStorageInitializer, MySqlStorageInitializer>();
        services.TryAddSingleton<IEasyQuartzJobStore, MySqlJobLogStore>();
        services.TryAddSingleton<IMonitoringApi, MySqlMonitoringApi>();

        //Add MySqlOptions
        services.Configure(_configure);
    } 
}