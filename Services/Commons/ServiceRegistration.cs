using LogBookAPI.Models;
using Microsoft.EntityFrameworkCore;
using LogBookAPI.Interfaces;
using System.Reflection;

namespace LogBookAPI.Services.Commons
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddRequiredServices(this IServiceCollection services, IConfiguration configuration, string environmentName){
            services.AddHttpContextAccessor();

            services.AddControllers().AddNewtonsoftJson(
                opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
            );

            // services.AddSwaggerGen(
            //     opt =>
            //     {
            //         var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //         opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            //     });

            services.AddSingleton<ILogger, Logger<LogbookContext>>();
            services.AddDbContext<LogbookContext>(
                opt =>
                {
                    opt.UseSqlServer(GetConnectionString(configuration));
                    if (!environmentName.Equals("Production")) opt.EnableSensitiveDataLogging();
                });

            services.AddScoped<IUserServices, UserService>();
            services.AddScoped<IRoleServices, RoleService>();
            services.AddScoped<ILogbookServices, LogbookService>();
            services.AddScoped<ILogbookItemService, LogbookItemService>();
            return services;
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connString = Environment.GetEnvironmentVariable("SqLConnectionString");
            if (!string.IsNullOrEmpty(connString))
            {
                return connString;
            }

            return configuration.GetConnectionString("SqLConnectionString");
        }
    }
}