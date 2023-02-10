using EasyQuartz.Monitoring;
using EasyQuartz.Storage;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace EasyQuartzStore;

public class MySqlMonitoringApi : IMonitoringApi
{
    private readonly MySqlOptions _options;
    private readonly string _jobName;
    private readonly string _logName;

    public MySqlMonitoringApi(IOptions<MySqlOptions> options, IStorageInitializer initializer)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _jobName = initializer.GetJobTableName();
        _logName = initializer.GetLogTableName();
    }

    public async Task<PagedQueryResult<JobDto>> GetJobsAsync(JobQueryDto queryDto)
    {
        var where = string.Empty;
        var sqlQuery =
            $"select * from `{_jobName}` where 1=1 {where} limit @Limit offset @Offset";
        object[] sqlParams =
        {
            new MySqlParameter("@Offset", queryDto.CurrentPage * queryDto.PageSize),
            new MySqlParameter("@Limit", queryDto.PageSize)
        };

        var connection = new MySqlConnection(_options.ConnectionString);
        await using var _ = connection.ConfigureAwait(false);

        var count = await connection.ExecuteScalarAsync<int>($"select count(1) from `{_jobName}` where 1=1 {where}");

        var items = await connection.ExecuteReaderAsync(sqlQuery, async reader =>
        {
            var messages = new List<JobDto>();

            while (await reader.ReadAsync())
            {
                var index = 0;
                messages.Add(new JobDto()
                {
                    Id = reader.GetString(index++),
                    JobKey = reader.GetString(index++),
                    JobDesc = reader.GetString(index++),
                    Cron = reader.GetString(index++),
                    LastFireTime = reader.GetDateTime(index++),
                    NextFireTime = reader.GetDateTime(index++),
                });
            }

            return messages;
        }, sqlParams).ConfigureAwait(false);

        return new PagedQueryResult<JobDto>
            { Items = items, PageIndex = queryDto.CurrentPage, PageSize = queryDto.PageSize, Totals = count };
    }

    public async Task<PagedQueryResult<LogDto>> GetLogsAsync(LogQueryDto queryDto)
    {
        var where = string.Empty;
        if (!string.IsNullOrEmpty(queryDto.JobKey)) where += " and JobKey=@JobKey";

        var sqlQuery =
            $"select * from `{_logName}` where 1=1 {where} order by fireTime desc limit @Limit offset @Offset";
        object[] sqlParams =
        {
            new MySqlParameter("@JobKey", queryDto.JobKey ?? string.Empty),
            new MySqlParameter("@Offset", queryDto.CurrentPage * queryDto.PageSize),
            new MySqlParameter("@Limit", queryDto.PageSize)
        };

        var connection = new MySqlConnection(_options.ConnectionString);
        await using var _ = connection.ConfigureAwait(false);

        var count = await connection.ExecuteScalarAsync<int>($"select count(1) from `{_logName}` where 1=1 {where}",
            new MySqlParameter("@JobKey", queryDto.JobKey ?? string.Empty));

        var items = await connection.ExecuteReaderAsync(sqlQuery, async reader =>
        {
            var messages = new List<LogDto>();

            while (await reader.ReadAsync())
            {
                var index = 0;
                messages.Add(new LogDto
                {
                    Id = reader.GetString(index++),
                    JobKey = reader.GetString(index++),
                    FireTime = reader.GetDateTime(index++),
                    RunTime = reader.GetInt32(index++),
                });
            }

            return messages;
        }, sqlParams).ConfigureAwait(false);

        return new PagedQueryResult<LogDto>
            { Items = items, PageIndex = queryDto.CurrentPage, PageSize = queryDto.PageSize, Totals = count };
    }
}