using System;

namespace EasyQuartz
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TriggerCronAttribute : Attribute
    {
        public TriggerCronAttribute(string cron)
        {
            Cron = string.IsNullOrWhiteSpace(cron) ? "0/1 * * * * ? *" : cron;
        }

        public string Cron { get; set; }
    }
}
