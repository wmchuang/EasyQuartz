using EasyQuartz.Options;

namespace EasyQuartzStore;

public static class EasyQuartzOptionsExtensions
{
    public static EasyQuartzOptions UseMySql(this EasyQuartzOptions options, string connectionString)
    {
        return options.UseMySql(opt => { opt.ConnectionString = connectionString; });
    }

    public static EasyQuartzOptions UseMySql(this EasyQuartzOptions options, Action<MySqlOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        options.RegisterExtension(new MySqlAuditingOptionsExtension(configure));
        return options;
    }
}