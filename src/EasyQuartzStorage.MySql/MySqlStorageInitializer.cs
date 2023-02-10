using EasyQuartz.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace EasyQuartzStore;

public class MySqlStorageInitializer : IStorageInitializer
{
    private readonly IOptions<MySqlOptions> _options;
    private readonly ILogger _logger;

    public MySqlStorageInitializer(
        ILogger<MySqlStorageInitializer> logger,
        IOptions<MySqlOptions> options)
    {
        _options = options;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var sql = CreateDbTablesScript();
        await using var connection = new MySqlConnection(_options.Value.ConnectionString);
        connection.ExecuteNonQuery(sql);

        _logger.LogDebug("Ensuring all create database tables script are applied.");
    }

    public string GetJobTableName()
    {
        return $"{_options.Value.TableNamePrefix}.job";
    }

    public string GetLogTableName()
    {
        return $"{_options.Value.TableNamePrefix}.log";
    }

    /// <summary>
    /// 获取创建表sql
    /// </summary>
    /// <returns></returns>
    protected virtual string CreateDbTablesScript()
    {
        var sql = $@"
            CREATE TABLE IF NOT EXISTS `{GetJobTableName()}` (
              `Id` varchar(36) NOT NULL COMMENT '主键',
              `JobKey`  varchar(50) NOT NULL,
              `JobDesc`  varchar(50) NOT NULL,
              `Cron`  varchar(20) NOT NULL DEFAULT  '',
              `LastFireTime` datetime,
              `NextFireTime` datetime,
              PRIMARY KEY (`Id`),
              UNIQUE  Key(`JobKey`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE IF NOT EXISTS `{GetLogTableName()}` (
              `Id` varchar(36) NOT NULL COMMENT '主键',
              `JobKey`  varchar(50) NOT NULL,
              `FireTime` datetime NOT NULL,
              `RunTime`  int(11),
              PRIMARY KEY (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            ";
        return sql;
    }
}