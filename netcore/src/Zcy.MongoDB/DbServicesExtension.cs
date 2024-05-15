using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Zcy.BaseInterface.Entities;

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
            //db utc  展示 local time
            BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);

            //todo:默认全局仓储，特殊单独注入单独的
            services.TryAdd(ServiceDescriptor.Transient(typeof(IBaseRepository<,>), typeof(CommonRepository<,>)));
            //services.AddTransient<IGptWebMessageRepository, GptWebMessageRepository>();
            //services.AddTransient<IActivationCodeRepository, ActivationCodeRepository>();
            //services.AddTransient<IPerUseActivationCodeRecordRepository, PerUseActivationCodeRecordRepository>();
            //services.AddTransient<IActivationCodeTypeV2Repository, ActivationCodeTypeV2Repository>();
            //services.AddTransient<IGptWebConfigRepository, GptWebConfigRepository>();
            services.AddTransient<IQueryableExecute, QueryableExecuteWithMongoDb>();
            return services;
        }
    }
}
