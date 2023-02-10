// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP;
using EasyQuartz.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace EasyQuartz.Dashboard
{
    internal sealed class DashboardOptionsExtension : IEasyQuartzOptionsExtension
    {
        private readonly Action<DashboardOptions> _options;

        public DashboardOptionsExtension(Action<DashboardOptions> option)
        {
            _options = option;
        }

        public void AddServices(IServiceCollection services)
        {
            var dashboardOptions = new DashboardOptions();
            _options?.Invoke(dashboardOptions);
            services.AddSingleton(dashboardOptions);
            
            services.AddTransient<IStartupFilter, StartupFilter>();
        }
    }

    sealed class StartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                next(app);

                app.UseDashboard();
            };
        }
    }
}