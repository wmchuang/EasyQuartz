using EasyQuartz.Monitoring;
using EasyQuartz.Storage;
using EasyQuartz.Storage.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace EasyQuartzStore;

public class MySqlJobLogStore : IEasyQuartzJobStore
{
    private readonly ILogger<MySqlJobLogStore> _logger;
    private readonly IOptions<MySqlOptions> _options;
    private readonly IStorageInitializer _initializer;
    private readonly string _jobName;
    private readonly string _logName;

    public MySqlJobLogStore(ILogger<MySqlJobLogStore> logger, IOptions<MySqlOptions> options, IStorageInitializer initializer)
    {
        _logger = logger;
        _options = options;
        _initializer = initializer;
        _jobName = initializer.GetJobTableName();
        _logName = initializer.GetLogTableName();
    }

    /// <see cref="IEasyQuartzJobStore.AddJobAsync"/>
    public async Task AddJobAsync(Job job)
    {
        try
        {
            var sql = $"INSERT Ignore INTO `{_jobName}`(Id, JobKey, JobDesc) " +
                      "VALUES(@Id, @JobKey, @JobDesc);";

            object[] sqlParams =
            {
                new MySqlParameter("@Id", job.Id),
                new MySqlParameter("@JobKey", job.JobKey),
                new MySqlParameter("@JobDesc", job.JobDesc)
            };
            await using var connection = new MySqlConnection(_options.Value.ConnectionString);
            connection.ExecuteNonQuery(sql, sqlParams: sqlParams);
        }
        catch (Exception e)
        {
            _logger.LogError($"新增任务时出错{e.Message}");
        }
    }

    /// <see cref="IEasyQuartzJobStore.SetJobFireAsync"/>
    public async Task SetJobFireAsync(string fireTime, string nextFireTime, string jobKey)
    {
        try
        {
            var sql = $"update `{_jobName}` set LastFireTime=@LastFireTime,NextFireTime=@NextFireTime where JobKey=@JobKey";
            if (string.IsNullOrWhiteSpace(nextFireTime))
            {
                sql = $"update `{_jobName}` set LastFireTime=@LastFireTime where JobKey=@JobKey";
            }

            object[] sqlParams =
            {
                new MySqlParameter("@LastFireTime", fireTime),
                new MySqlParameter("@NextFireTime", nextFireTime),
                new MySqlParameter("@JobKey", jobKey),
            };
            await using var connection = new MySqlConnection(_options.Value.ConnectionString);
            connection.ExecuteNonQuery(sql, sqlParams: sqlParams);
        }
        catch (Exception e)
        {
            _logger.LogError($"设置任务执行时间信息时出错{e.Message}");
        }
    }

    /// <see cref="IEasyQuartzJobStore.SetJobCronAsync"/>
    public async Task SetJobCronAsync(string cron, string jobKey)
    {
        try
        {
            var sql = $"update `{_jobName}` set Cron=@Cron where JobKey=@JobKey";

            object[] sqlParams =
            {
                new MySqlParameter("@Cron", cron),
                new MySqlParameter("@JobKey", jobKey),
            };
            await using var connection = new MySqlConnection(_options.Value.ConnectionString);
            connection.ExecuteNonQuery(sql, sqlParams: sqlParams);
        }
        catch (Exception e)
        {
            _logger.LogError($"设置任务设置的Cron时出错{e.Message}");
        }
    }

    /// <see cref="IEasyQuartzJobStore.SaveLogAsync"/>
    public async Task SaveLogAsync(JobLog jobLog)
    {
        try
        {
            var sql = $"INSERT ignore INTO `{_logName}`(Id, JobKey, FireTime, RunTime) " +
                      "VALUES(@Id, @JobKey, @FireTime, @RunTime );";

            object[] sqlParams =
            {
                new MySqlParameter("@Id", jobLog.Id),
                new MySqlParameter("@JobKey", jobLog.JobKey),
                new MySqlParameter("@FireTime", jobLog.FireTime),
                new MySqlParameter("@RunTime", jobLog.RunTime),
            };
            await using var connection = new MySqlConnection(_options.Value.ConnectionString);
            connection.ExecuteNonQuery(sql, sqlParams: sqlParams);
        }
        catch (Exception e)
        {
            _logger.LogError($"保存任务日志时出错{e.Message}");
        }
    }

    /// <see cref="IEasyQuartzJobStore.DeleteExpiresAsync"/>
    public async Task<int> DeleteExpiresAsync(DateTime timeout, int batchCount = 1000, CancellationToken token = default)
    {
        await using var connection = new MySqlConnection(_options.Value.ConnectionString);
        return connection.ExecuteNonQuery(
            $@"DELETE FROM `{_logName}` WHERE FireTime < @timeout limit @batchCount;", null,
            new MySqlParameter("@timeout", timeout), new MySqlParameter("@batchCount", batchCount));
    }

    public IMonitoringApi GetMonitoringApi()
    {
        return new MySqlMonitoringApi(_options, _initializer);
    }
}