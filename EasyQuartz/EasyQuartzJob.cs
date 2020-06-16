namespace EasyQuartz
{
    public abstract class EasyQuartzJob
    {
        public abstract string Cron { get; }
    }
}
