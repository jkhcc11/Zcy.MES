using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Products;
using Zcy.IRepository.FinancialMemo;
using Zcy.IRepository.Production;
using Zcy.IRepository.Products;
using Zcy.IRepository.SysBaseInfo;
using Zcy.IRepository.User;
using Zcy.MongoDB.FinancialMemo;
using Zcy.MongoDB.Production;
using Zcy.MongoDB.Products;
using Zcy.MongoDB.SysBaseInfo;
using Zcy.MongoDB.User;

namespace Zcy.MongoDB
{
    public static class DbServicesExtension
    {
        /// <summary>
        /// 添加Mongodb仓储 DI
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddMongodbRepository(this IServiceCollection services)
        {
            SetMongodbMap();

            //db utc  展示 local time
            BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
            // 注册自定义的 decimal 序列化器
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer());

            //todo:默认全局仓储，特殊单独注入单独的
            services.TryAdd(ServiceDescriptor.Transient(typeof(IBaseRepository<,>), typeof(CommonRepository<,>)));

            services.AddTransient<ISystemUserRepository, SystemUserRepository>();

            services.AddTransient<ISystemMenuRepository, SystemMenuRepository>();
            services.AddTransient<ISystemRoleMenuRepository, SystemRoleMenuRepository>();

            services.AddTransient<IProceedsRecordRepository, ProceedsRecordRepository>();

            services.AddTransient<IProductCraftRepository, ProductCraftRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddTransient<IReportWorkRepository, ReportWorkRepository>();
            //services.AddTransient<IActivationCodeRepository, ActivationCodeRepository>();
            //services.AddTransient<IPerUseActivationCodeRecordRepository, PerUseActivationCodeRecordRepository>();
            //services.AddTransient<IActivationCodeTypeV2Repository, ActivationCodeTypeV2Repository>();
            //services.AddTransient<IGptWebConfigRepository, GptWebConfigRepository>();
            services.AddTransient<IQueryableExecute, QueryableExecuteWithMongoDb>();
            return services;
        }

        private static void SetMongodbMap()
        {
            // 配置类映射
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ProductProcesses); // 忽略
            });

            BsonClassMap.RegisterClassMap<ProductProcess>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Product);
                cm.UnmapMember(c => c.ProductCraft); // 忽略
            });
        }
    }
}
