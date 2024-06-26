using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Snowflake.Core;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.BaseInterface.Service;
using Zcy.Dto;
using Zcy.ExcelNpoi;
using Zcy.IService.SysBaseInfo;
using Zcy.IService.User;
using Zcy.MongoDB;
using Zcy.Service.SysBaseInfo;
using Zcy.Service.User;

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
            //todo:记得先清理默认
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
                        RoleClaimType = JwtClaimTypes.Role,
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

            //id生成
            services.AddSingleton(_ => new IdGenerateExtension(new IdWorker(1, 1)));

            services.AddScoped<ILoginUserInfo, LoginUserInfo>();

            services.AddAllService();

            services.AddZcyExcelService();
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

        /// <summary>
        /// 添加Redis
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddRedis(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings:RedisConnStr");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("未配置Redis连接信息，请检查ConnectionStrings:RedisConnStr");
            }

            // 配置Redis分布式缓存
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString; // Redis服务器地址
                options.InstanceName = "ZcyMes:";
            });
            return services;
        }

        /// <summary>
        /// 服务自动注入
        /// </summary>
        /// <returns></returns>
        internal static IServiceCollection AddAllService(this IServiceCollection services)
        {
            //加载当前项目程序集
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Zcy.*.dll")
                .Select(Assembly.LoadFrom)
                .ToArray();

            //所有程序集类型声明
            var allTypes = new List<Type>();
            foreach (var itemAssemblies in assemblies)
            {
                allTypes.AddRange(itemAssemblies.GetTypes());
            }
            //公用的接口
            var baseType = typeof(IZcyBaseService);
            //过滤需要用到的服务声明接口
            var useType = allTypes.Where(a => baseType.IsAssignableFrom(a) && a.IsAbstract == false).ToList();
            foreach (var item in useType)
            {
                //该服务所属接口
                var currentInterface = item.GetInterfaces().FirstOrDefault(a => a.Name.EndsWith(item.Name));
                if (currentInterface == null)
                {
                    continue;
                }

                //每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
                services.AddTransient(currentInterface, item);
            }

            return services;
        }
    }
}
