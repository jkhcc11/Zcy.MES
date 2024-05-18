using Microsoft.Extensions.DependencyInjection;

namespace Zcy.BaseInterface.Service
{
    /// <summary>
    /// 基础服务接口
    /// </summary>
    /// <remarks>
    ///  继承这个接口的服务会自动注入为:AddTransient
    /// </remarks>
    public interface IZcyBaseService
    {
        public IServiceCollection ServiceCollection { get; set; }
    }
}
