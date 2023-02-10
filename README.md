# EasyQuartz
### 1. Nuget安装包 EasyQuartz



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

### 4. 数据持久化和控制面板

##### 需要引入包 EasyQuartzStorage.MySql 和 EasyQuartz.Dashboard
	 using EasyQuartz;
     builder.Services.AddEasyQuartz(e =>
     {
      e.UseMySql(m => { m.ConnectionString = "server=XXX;user=root;database='XXX';port=3306;password=XXX;SslMode=None"; });
      e.UseDashboard();
     });

##### 默认通过 http://ip:端口号/easyjob 访问
