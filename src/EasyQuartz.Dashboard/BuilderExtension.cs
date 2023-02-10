// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using EasyQuartz.Dashboard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace DotNetCore.CAP
{
    public static class CapBuilderExtension
    {
        internal static IApplicationBuilder UseDashboard(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var provider = app.ApplicationServices;

            var options = provider.GetService<DashboardOptions>();

            if (options != null)
            {
                app.UseMiddleware<UiMiddleware>();
                //
                // var metrics = provider.GetRequiredService<CapMetricsEventListener>();
                app.Map(options.PathMatch + "/api", false, x =>
                {
                    IAuthorizationService authService = null;
                    if (!string.IsNullOrEmpty(options.AuthorizationPolicy))
                    {
                        authService = app.ApplicationServices.GetService<IAuthorizationService>();
                    }

                    var builder = new RouteBuilder(x);

                    var methods = typeof(RouteActionProvider).GetMethods(BindingFlags.Instance | BindingFlags.Public);

                    foreach (var method in methods)
                    {
                        var executor = ObjectMethodExecutor.Create(method, typeof(RouteActionProvider).GetTypeInfo());

                        var getAttr = method.GetCustomAttribute<HttpGetAttribute>();
                        if (getAttr != null)
                        {
                            builder.MapGet(getAttr.Template, async (request, response, data) =>
                            {
                                var actionProvider = new RouteActionProvider(request, response, data);
                                try
                                {
                                    await executor.ExecuteAsync(actionProvider, null);
                                }
                                catch (Exception ex)
                                {
                                    response.StatusCode = StatusCodes.Status500InternalServerError;
                                    await response.WriteAsync(ex.Message);
                                }
                            });
                        }
                    }

                    var capRouter = builder.Build();

                    x.UseRouter(capRouter);
                });
            }

            return app;
        }
    }
}