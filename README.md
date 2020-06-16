# EasyQuartz
### 1. 引用EasyQuartz



### 2. Startup Add Service
	 using EasyQuartz;
     public void ConfigureServices(IServiceCollection services)
     {
            //Add Service
            services.AddEasyQuartzService();
     }
### 3. Create Job
##### 第一种方式，通过特性指定Cron
	[TriggerCron("0/1 * * * * ? *")]
    public class Test1Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test1Job");
            return Task.CompletedTask;
        }
    }
##### 第二种方式，通过继承EasyQuartzJob 来指定Cron
	public class Test2Job : EasyQuartzJob, IJob
    {
        private readonly IConfiguration _configuration;

        public Test2Job(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override string Cron => _configuration["Test2JobCron"];

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test2Job");
            return Task.CompletedTask;
        }
    }
