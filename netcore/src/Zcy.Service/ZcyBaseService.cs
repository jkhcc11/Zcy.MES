using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zcy.BaseInterface.Service;

namespace Zcy.Service
{
    /// <summary>
    /// 基础服务
    /// </summary>
    public abstract class ZcyBaseService : IZcyBaseService
    {
        public IServiceCollection ServiceCollection { get; set; }
        public IConfiguration Configuration { get; set; }
        protected readonly ILoginUserInfo LoginUserInfo;
        protected readonly IMapper BaseMapper;

        protected ZcyBaseService(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            var serviceProvider = serviceCollection.BuildServiceProvider();
            LoginUserInfo = serviceProvider.GetService<ILoginUserInfo>() ??
                            throw new ArgumentException("ZcyBaseService LoginUserInfo is null");
            BaseMapper = serviceProvider.GetService<IMapper>() ??
                         throw new ArgumentException("ZcyBaseService BaseMapper is null");
            Configuration = serviceProvider.GetService<IConfiguration>() ??
                           throw new ArgumentException("ZcyBaseService Configuration is null");
        }

    }
}
