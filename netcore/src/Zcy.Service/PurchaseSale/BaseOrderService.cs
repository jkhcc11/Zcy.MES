using Microsoft.Extensions.Caching.Distributed;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.PurchaseSale;
using Zcy.Entity.Company;
using Zcy.Entity.Products;
using Zcy.Entity.PurchaseSale;
using Zcy.Utility;

namespace Zcy.Service.PurchaseSale
{
    /// <summary>
    /// 基础订单服务
    /// </summary>
    public abstract class BaseOrderService : ZcyBaseService
    {
        /// <summary>
        /// 是否每天重置次数 //todo:这里根据业务情况调整，是按每天重新从1开始
        /// </summary>
        private const bool IsDayResetCount = false;
        /// <summary>
        /// 订单序号长度
        /// </summary>
        protected const int OrderNoIndexLength = 5;

        /// <summary>
        /// 获取当前类型订单最新序号
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityRepository"></param>
        /// <param name="companyCacheItem">公司缓存信息</param>
        /// <param name="orderNoIndexLength">订单序号长度</param>
        /// <returns></returns>
        private async Task<int> GetOrderNoIndexAsync<TEntity>(
            IBaseRepository<TEntity, long> entityRepository,
            CompanyCacheItem companyCacheItem,
            int orderNoIndexLength = OrderNoIndexLength) where TEntity : BaseOrder
        {
            var noIndex = 0;

            //当天最新订单号
            var currentDate = DateTime.Today;
            var query = await entityRepository.GetQueryableAsync();
            if (IsDayResetCount)
            {
                query = query.Where(a => a.OrderDate == currentDate);
            }

            var currentNewQuery = query
                .Where(a => a.CompanyId == companyCacheItem.CompanyId)
                .OrderByDescending(a => a.CreatedTime);
            var currentNew = await entityRepository.FirstOrDefaultAsync(currentNewQuery);
            if (currentNew == null)
            {
                return noIndex;
            }

            //解析订单号得到序号
            noIndex = currentNew.OrderNo.GetOrderNoIndex(orderNoIndexLength);
            return noIndex;
        }

        /// <summary>
        /// 生成完整订单号
        /// </summary>
        /// <remarks>
        ///  订单号前缀（2）+ 公司缩写（2-4）+校验位（1）+ 订单号长度（动态配置）
        /// </remarks>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="entityRepository"></param>
        /// <param name="companyCacheItem">公司缓存信息</param>
        /// <param name="orderNoPrefix">完整订单号前缀</param>
        /// <param name="orderNoIndexLength">订单序号长度</param>
        /// <returns></returns>
        protected virtual async Task<string> GeneralFullOrderNoAsync<TEntity>(
            IDistributedCache distributedCache,
            IBaseRepository<TEntity, long> entityRepository,
            CompanyCacheItem companyCacheItem,
            string orderNoPrefix,
            int orderNoIndexLength = OrderNoIndexLength) where TEntity : BaseOrder
        {
            var fullOrderNoPrefix = $"{orderNoPrefix}{companyCacheItem.ShortName}{DateTime.Now:yyyyMMddHHmm}";
            var cacheKey = $"{orderNoPrefix}:{companyCacheItem.CompanyId}";
            if (IsDayResetCount)
            {
                cacheKey = $"{orderNoPrefix}:{companyCacheItem.CompanyId}:{DateTime.Today}";
            }

            int noIndex;
            var noIndexCache = await distributedCache.GetStringAsync(cacheKey);
            if (string.IsNullOrEmpty(noIndexCache))
            {
                noIndex = await GetOrderNoIndexAsync(entityRepository,
                    companyCacheItem,
                    orderNoIndexLength);
            }
            else
            {
                noIndex = noIndexCache.TryToInt32();
            }

            noIndex++;
            var maxOrderNumber = (int)Math.Pow(10, orderNoIndexLength) - 1;
            if (noIndex > maxOrderNumber)
            {
                noIndex = 1;
            }

            //缓存最新
            await distributedCache.SetStringAsync(cacheKey, noIndex + "",
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow
                     = TimeSpan.FromDays(30)
                });
            //返回完整订单号
            return
                $"{fullOrderNoPrefix}{OrderNoExt.CalculateCheckDigit(fullOrderNoPrefix)}{noIndex.ToOrderNo(orderNoIndexLength)}";
        }

        /// <summary>
        /// 校验订单详情产品信息
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidOrderDetailProductAsync<TInput>(
            List<Product> dbProducts,
            List<TInput>? orderDetailItems)
        where TInput : BaseCreateOrderDetailInput
        {
            if (orderDetailItems == null ||
                orderDetailItems.Any() == false)
            {
                return false;
            }

            var productIds = orderDetailItems.Select(a => a.ProductId).ToArray();
            return dbProducts.Count == productIds.Length;
        }
    }
}
