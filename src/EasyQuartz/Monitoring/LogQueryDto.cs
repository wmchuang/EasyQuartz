namespace EasyQuartz.Monitoring;

public class LogQueryDto
{
    public string JobKey { get; set; }
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}