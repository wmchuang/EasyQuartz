using System;
using System.Threading.Tasks;

namespace EasyQuartz
{
    public interface IJobManager
    {

        Task AddJobAsync(Type jobType, string cron, string id = "");
        Task RemoveJobAsync(Type jobType, string id = "");
        Task PauseJob(Type jobType, string id = "");
        Task OperateJob(Type jobType,OperateEnum operate, string id = "");
    }
}