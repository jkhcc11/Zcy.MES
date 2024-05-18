using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Zcy.BaseInterface
{
    public static class CacheExtension
    {
        /// <summary>
        /// 获取或创建缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key">缓存Key</param>
        /// <param name="factory">获取缓存方法</param>
        /// <param name="options">缓存配置</param>
        /// <returns></returns>
        public static async Task<T?> GetOrCreateAsync<T>(this IDistributedCache cache,
            string key, Func<Task<T>> factory,
            DistributedCacheEntryOptions? options = null)
        where T : class
        {
            // 尝试从缓存中获取数据
            var data = await cache.GetAsync(key);
            if (data != null)
            {
                // 如果数据存在，则反序列化并返回
                var jsonData = Encoding.UTF8.GetString(data);
                return JsonSerializer.Deserialize<T>(jsonData);
            }

            // 如果缓存中没有数据，则使用工厂方法生成数据
            var value = await factory();
            // 将生成的数据序列化并存入缓存
            var serializedData = JsonSerializer.Serialize(value);
            var bytes = Encoding.UTF8.GetBytes(serializedData);

            // 使用提供的缓存项配置选项，如果没有提供则使用默认选项
            options ??= new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await cache.SetAsync(key, bytes, options);
            return value;
        }
    }
}
