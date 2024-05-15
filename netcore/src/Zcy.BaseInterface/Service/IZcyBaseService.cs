using Microsoft.Extensions.DependencyInjection;

namespace Zcy.BaseInterface.Service
{
    /// <summary>
    /// 基础服务接口
    /// </summary>
    public interface IZcyBaseService
    {
        public IServiceCollection ServiceCollection { get; set; }
    }
}
