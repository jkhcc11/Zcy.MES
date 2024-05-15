using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Zcy.BaseInterface;
using Zcy.Dto;
using Zcy.MongoDB;

namespace Zcy.MES.HttpService
{
    public static class ServicesExtension
    {
        /// <summary>
        /// 添加JWT Auth
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            // 配置JWT认证
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        //令牌将被发送到哪里，即你的API或应用程序的地址
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        //ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            // 添加授权
            services.AddAuthorization(options =>
            {
                //manager
                options.AddPolicy(AuthorizationConst.NormalPolicyName.ManagerPolicy,
                    policy => policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == JwtClaimTypes.Role &&
                                                   (c.Value == AuthorizationConst.NormalRoleName.Admin ||
                                                    c.Value == AuthorizationConst.NormalRoleName.Boss ||
                                                    c.Value == AuthorizationConst.NormalRoleName.SuperAdmin))
                    ));
            });
            return services;
        }

        /// <summary>
        /// 添加Services DI
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("normal", new OpenApiInfo
                {
                    Title = "公用Api",
                    Version = "v2"
                });

                config.SwaggerDoc("login", new OpenApiInfo
                {
                    Title = "登录Api",
                    Version = "v2"
                });

                config.SwaggerDoc("manager", new OpenApiInfo
                {
                    Title = "管理Api",
                    Version = "v2"
                });

                // 定义Bearer认证方案
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                          Enter 'Bearer' [space] and then your token in the text input below.
                          Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            //services.AddTransient<IOpenAiDashboardApiHttpApi, OpenAiDashboardApiHttpApi>();
            //services.AddTransient<IOpenAiHttpApi, OpenAiHttpApi>();

            //services.AddTransient<IActivationCodeService, ActivationCodeService>();
            //services.AddTransient<IWebConfigService, WebConfigService>();

            //services.AddTransient<IWebConfigAdminService, WebConfigAdminService>();
            //services.AddTransient<IActivationCodeAdminService, ActivationCodeAdminService>();
            return services;
        }

        /// <summary>
        /// 添加Mongodb
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddMongodb(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings:ConnStr");
            var databaseName = configuration.GetValue<string>("ConnectionStrings:MongodbDatabaseName");
            if (string.IsNullOrEmpty(connectionString) ||
                string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("未配置Mongodb连接信息，请检查ConnectionStrings:Mongodb、ConnectionStrings:MongodbDatabaseName");
            }

            services.AddSingleton(_ =>
                new ZcyMongodbContext(new MongoClient(connectionString), databaseName, services));
            services.AddMongodbRepository();
            return services;
        }
    }
}
