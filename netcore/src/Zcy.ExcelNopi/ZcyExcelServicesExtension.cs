using Microsoft.Extensions.DependencyInjection;

namespace Zcy.ExcelNpoi
{
	/// <summary>
	///Excel 扩展模块
	/// </summary>
	public static class ZcyExcelServicesExtension
	{
        /// <summary>
        /// 添加Excel DI
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddZcyExcelService(this IServiceCollection services)
        {
            services.AddSingleton<IZcyExcelExtension, ZcyExcelExtension>();
            return services;
        }

    }
}
