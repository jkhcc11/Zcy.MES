
using Microsoft.OpenApi.Models;
using Zcy.MES.HttpService;

namespace Zcy.MES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddServices();
            builder.Services.AddJwtAuth(builder.Configuration);

            var app = builder.Build();

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
    }
}
