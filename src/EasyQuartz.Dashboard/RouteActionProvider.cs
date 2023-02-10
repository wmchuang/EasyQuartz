using System.Text.Json;
using EasyQuartz.Monitoring;
using EasyQuartz.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace EasyQuartz.Dashboard
{
    public class RouteActionProvider
    {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;
        private readonly RouteData _routeData;
        private IServiceProvider ServiceProvider => _request.HttpContext.RequestServices;
        private IMonitoringApi MonitoringApi => ServiceProvider.GetRequiredService<IEasyQuartzJobStore>().GetMonitoringApi();
        
        private  IJobManager _jobManager => ServiceProvider.GetRequiredService<IJobManager>();

        private JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            Converters = { new DatetimeJsonConverter() },
            PropertyNamingPolicy =JsonNamingPolicy.CamelCase
        };

        public RouteActionProvider(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            _request = request;
            _response = response;
            _routeData = routeData;
            _response.StatusCode = StatusCodes.Status200OK;
        }

        [HttpGet("/jobs")]
        public async Task JobList()
        {
            var routeValue = _routeData.Values;
            var pageSize = _request.Query["perPage"].ToInt32OrDefault(20);
            var pageIndex = _request.Query["currentPage"].ToInt32OrDefault(1);
            var name = _request.Query["name"].ToString();

            var queryDto = new JobQueryDto
            {
                CurrentPage = pageIndex - 1,
                PageSize = pageSize
            };

            var result = await MonitoringApi.GetJobsAsync(queryDto);
            await _response.WriteAsJsonAsync(result, Options);
        }
        
        [HttpGet("/run")]
        public async Task RunJob()
        {
            var jobKey = _request.Query["jobKey"].ToString();
            await _jobManager.RunJobAsync(jobKey);
            
            await _response.WriteAsync("执行成功");
        }

        [HttpGet("/logs")]
        public async Task LogList()
        {
            var routeValue = _routeData.Values;
            var pageSize = _request.Query["perPage"].ToInt32OrDefault(20);
            var pageIndex = _request.Query["currentPage"].ToInt32OrDefault(1);
            var jobKey = _request.Query["jobKey"].ToString();

            var queryDto = new LogQueryDto
            {
                CurrentPage = pageIndex - 1,
                PageSize = pageSize,
                JobKey = jobKey
            };

            var result = await MonitoringApi.GetLogsAsync(queryDto);
            await _response.WriteAsJsonAsync(result, Options);
        }

        [HttpGet("/health")]
        public Task Health()
        {
            _response.WriteAsync("OK");
            return Task.CompletedTask;
        }

        private void BadRequest()
        {
            _response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public static class IntExtension
    {
        public static int ToInt32OrDefault(this StringValues value, int defaultValue = 0)
        {
            return int.TryParse(value, out var result) ? result : defaultValue;
        }
    }
}