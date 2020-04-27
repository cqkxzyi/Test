// ======================================================================
// 
//           Copyright (C) 2019-2020 ����������Ϣ�Ƽ����޹�˾
//           All rights reserved
// 
//           filename : Startup.cs
//           description :
// 
//           created by ѩ�� at  2019-06-14 11:22
//           �����ĵ�: docs.xin-lai.com
//           ���ںŽ̳̣�magiccodes
//           QQȺ��85318032����̽�����
//           Blog��http://www.cnblogs.com/codelove/
//           Home��http://xin-lai.com
// 
// ======================================================================

using System;
using System.IO;
using System.Linq;
using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Abp.PlugIns;
using Castle.Facilities.Logging;
using Magicodes.Admin.Core.Configuration;
using Magicodes.Admin.Core.Identity;
using Magicodes.Admin.EntityFrameworkCore.EntityFramework;
using Magicodes.Admin.Web.Core.Chat.SignalR;
using Magicodes.Admin.Web.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Abp.Castle.Logging.NLog;
using HealthChecks.UI.Client;
using Magicodes.Admin.Web.Core.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;

namespace Magicodes.Admin.Web.Host.Startup
{
    public partial class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
            _logger = logger;
            //��ӡ��Ҫ������Ϣ
            _logger.LogInformation($"Environment:{env.EnvironmentName}{Environment.NewLine}" +
                                   $"ConnectionString:{_appConfiguration["ConnectionStrings:Default"]}{Environment.NewLine}" +
                                   $"RedisCache:IsEnabled:{_appConfiguration["Abp:RedisCache:IsEnabled"]}  ConnectionString:{_appConfiguration["Abp:RedisCache:ConnectionString"]}{Environment.NewLine}" +
                                   $"SignalRRedisCache:{_appConfiguration["Abp:SignalRRedisCache:ConnectionString"]}{Environment.NewLine}" +
                                   $"HTTPS:HttpsRedirection:{_appConfiguration["App:HttpsRedirection"]}  UseHsts:{_appConfiguration["App:UseHsts"]}{Environment.NewLine}" +
                                   $"CorsOrigins:{_appConfiguration["App:CorsOrigins"]}{Environment.NewLine}");
        }

        /// <summary>
        ///     �����Զ������
        /// </summary>
        /// <param name="services"></param>
        partial void ConfigureCustomServices(IServiceCollection services);

        partial void CustomConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(DefaultCorsPolicyName));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var sbuilder = services.AddSignalR(options => { options.EnableDetailedErrors = true; });

            if (!_appConfiguration["Abp:SignalRRedisCache:ConnectionString"].IsNullOrWhiteSpace())
            {
                _logger.LogWarning("Abp:SignalRRedisCache:ConnectionString:" +
                                   _appConfiguration["Abp:SignalRRedisCache:ConnectionString"]);
                sbuilder.AddRedis(_appConfiguration["Abp:SignalRRedisCache:ConnectionString"]);
            }

            //Configure CORS for APP
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            if (bool.Parse(_appConfiguration["App:HttpsRedirection"] ?? "false"))
            {
                //���鿪���������������ʾ��ȫͼ��
                //����https�ض���˿�
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = 443;
                });
            }

            //�Ƿ�����HTTP�ϸ��䰲ȫЭ��(HSTS)
            if (bool.Parse(_appConfiguration["App:UseHsts"] ?? "false"))
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(60);
                    options.ExcludedHosts.Add("example.com");
                });
            }

            try
            {
                _logger.LogWarning("ConfigureCustomServices  Begin...");
                ConfigureCustomServices(services);
                _logger.LogWarning("ConfigureCustomServices  End...");
            }
            catch (Exception ex)
            {
                _logger.LogError("ִ��ConfigureCustomServices���ִ���", ex);
            }

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"] ?? "false"))
            {
                services.AddAdminHealthChecks();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<HealthChecksUISettings>(settings =>
                    {
                        healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true);
                    });
                    services.AddHealthChecksUI();
                }
            }

            try
            {
                _logger.LogWarning("abp  Begin...");
                //����ABP�Լ����ģ������
                return services.AddAbp<AdminWebHostModule>(options =>
                {
                    options.IocManager.Register<IAppConfigurationAccessor, AppConfigurationAccessor>();

                    //������־
                    options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                        f =>
                        {
                            var logType = _appConfiguration["Abp:LogType"];
                            _logger.LogInformation($"LogType:{logType}");
                            if (logType != null && logType == "NLog")
                            {
                                f.UseAbpNLog().WithConfig("nlog.config");
                            }
                            else
                            {
                                f.UseAbpLog4Net().WithConfig("log4net.config");
                            }
                        });

                    if (Convert.ToBoolean(_appConfiguration["Abp:PlugInSources"] ??
                                          "false"))
                    {
                        //Ĭ�ϲ��������Ŀ¼�����Ƽ����ģʽ��
                        options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"), SearchOption.AllDirectories);
                    }

                });
            }
            catch (Exception ex)
            {
                _logger.LogError("����Abp���ִ���", ex);
                return null;
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"] ?? "false"))
            {
                app.UseHealthChecks("/healthz", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                {
                    app.UseHealthChecksUI();
                }
            }

            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(DefaultCorsPolicyName); //Enable CORS!

            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"] ?? "false"))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseStaticFiles();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<DatabaseCheckHelper>()
                    .Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    app.UseAbpRequestLocalization();
                }
            }

            //app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
                routes.MapHub<ChatHub>("/signalr-chat");
                ////ʹ�ó���ѯ
                //routes.MapHub<AbpCommonHub>("/signalr", otp => otp.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling);
                //routes.MapHub<ChatHub>("/signalr-chat", otp => otp.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling);
            });

            if (bool.Parse(_appConfiguration["App:HttpsRedirection"] ?? "false"))
            {
                _logger.LogWarning("׼������HTTS��ת...");
                //���鿪���������������ʾ��ȫͼ��
                app.UseHttpsRedirection();
            }

            //�Ƿ�����HTTP�ϸ��䰲ȫЭ��(HSTS)�����������رա�
            if (!env.IsDevelopment() && bool.Parse(_appConfiguration["App:UseHsts"] ?? "false"))
            {
                try
                {
                    app.UseHsts();
                }
                catch (Exception ex)
                {
                    _logger.LogError("����HSTS���ִ���", ex);
                }
            }

            try
            {
                _logger.LogWarning("Ӧ���Զ�������...");
                CustomConfigure(app, env, loggerFactory);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ӧ���Զ������ó��ִ���", ex);
            }
        }
    }
}