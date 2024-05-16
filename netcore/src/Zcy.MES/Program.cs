using Serilog;
using Zcy.BaseInterface;
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
                builder.Services.AddControllers()
                    .AddJsonOptions(conf =>
                    {
                        conf.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        conf.JsonSerializerOptions.Converters.Add(new LongConverter());
                    });
                builder.Services.AddMemoryCache();
                builder.Services.AddHttpContextAccessor();
                AuthorizationConst.ServiceCollection = builder.Services;
                builder.Services.AddMongodb(builder.Configuration);
                builder.Services.AddServices();
                builder.Services.AddJwtAuth(builder.Configuration);

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

                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "≥Ã–Ú∆Ù∂Ø“Ï≥£");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
