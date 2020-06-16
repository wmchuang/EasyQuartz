using System;

namespace EasyQuartz
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, string group = "defaultGroup")
        {
            JobType = jobType;
            CronExpression = cronExpression;
            Group = group;
        }

        public Type JobType { get; }

        public string CronExpression { get; }

        public string Group { get; set; }
    }
}
