using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using Zcy.BaseInterface;
using Zcy.MES.Filter;
using Zcy.MES.HttpService;
using Zcy.MES.JsonConvert;

namespace Zcy.MES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting ZcyMes application");
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddSerilog();

                //�Զ���ģ��У��
                builder.Services.Configure<ApiBehaviorOptions>(option =>
                {
                    option.SuppressModelStateInvalidFilter = true;
                });
                builder.Services.AddControllersWithViews(options => { options.Filters.Add<ModelStateValidFilter>(); });

                builder.Services.AddControllers()
                    .AddJsonOptions(conf =>
                    {
                        conf.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        conf.JsonSerializerOptions.Converters.Add(new LongConverter());
                    });
                builder.Services.AddMemoryCache();
                builder.Services.AddRedis(builder.Configuration);
                builder.Services.AddHttpContextAccessor();
                AuthorizationConst.ServiceCollection = builder.Services;
                builder.Services.AddMongodb(builder.Configuration);
                builder.Services.AddServices();
                builder.Services.AddJwtAuth(builder.Configuration);

                //cros
                var defaultPolicy = "DefaultCorsPolicy";
                var corsHost = builder.Configuration.GetValue<string>("CorsHosts") ?? "http://localhost:5173";
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(defaultPolicy, corsBuilder =>
                    {
                        corsBuilder.WithOrigins(corsHost.Split(","))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithExposedHeaders("Content-Disposition"); //�����ļ�ʱ ��Ҫͨ���������ȡ����
                    });
                });

                var app = builder.Build();

                app.UseSerilogRequestLogging();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/normal/swagger.json", "normal");
                        c.SwaggerEndpoint("/swagger/login/swagger.json", "login");
                        c.SwaggerEndpoint("/swagger/manager/swagger.json", "manager");
                    });
                }

                //todo:��������������Ȩ֮ǰ ������Ȩʧ�ܿ�������
                app.UseCors(defaultPolicy);
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseKdyLog();
                app.MapControllers();

                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "���������쳣");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
