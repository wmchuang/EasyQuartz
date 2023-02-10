using System.Threading.Tasks;

namespace EasyQuartz.Monitoring;

public interface IMonitoringApi
{
    Task<PagedQueryResult<JobDto>> GetJobsAsync(JobQueryDto queryDto);

    Task<PagedQueryResult<LogDto>> GetLogsAsync(LogQueryDto queryDto);
}