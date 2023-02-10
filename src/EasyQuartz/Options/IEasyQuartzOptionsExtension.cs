using Microsoft.Extensions.DependencyInjection;

namespace EasyQuartz.Options;

public interface IEasyQuartzOptionsExtension
{
    /// <summary>
    /// Registered child service.
    /// </summary>
    /// <param name="services">add service to the <see cref="IServiceCollection" /></param>
    void AddServices(IServiceCollection services);
}