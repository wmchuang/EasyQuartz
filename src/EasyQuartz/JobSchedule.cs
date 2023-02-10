using System;

namespace EasyQuartz
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, string jobDesc, string group = "defaultGroup", bool startNow = false)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            JobDesc = jobDesc;
            Group = group;
            StartNow = startNow;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public Type JobType { get; }
        
        /// <summary>
        /// 任务描述
        /// </summary>
        public string JobDesc { get; }

        /// <summary>
        /// 立即启动
        /// </summary>
        public bool StartNow { get; }
        
        /// <summary>
        /// Cron
        /// </summary>
        public string CronExpression { get; }

        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; }
    }
}
