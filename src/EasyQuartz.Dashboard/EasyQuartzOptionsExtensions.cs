using EasyQuartz.Dashboard;
using EasyQuartz.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class EasyQuartzOptionsExtensions
{
    public static EasyQuartzOptions UseDashboard(this EasyQuartzOptions capOptions)
    {
        return capOptions.UseDashboard(opt => { });
    }

    public static EasyQuartzOptions UseDashboard(this EasyQuartzOptions capOptions, Action<DashboardOptions> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        capOptions.RegisterExtension(new DashboardOptionsExtension(options));

        return capOptions;
    }
}