using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.DataReport;
using Zcy.IRepository.PurchaseSale;
using Zcy.IService.DataReport;

namespace Zcy.Service.DataReport
{
    /// <summary>
    /// 产品报表 服务实现
    /// </summary>
    public class ProductReportService : ZcyBaseService, IProductReportService
    {
        private readonly ISaleOrderDetailRepository _saleOrderDetailRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;

        public ProductReportService(ISaleOrderDetailRepository saleOrderDetailRepository,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository)
        {
            _saleOrderDetailRepository = saleOrderDetailRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        }

        /// <summary>
        /// 分页查询产品销售报表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageProductSaleReportDto>>> QueryPageProductSaleReportAsync(
            QueryPageProductSaleReportInput input)
        {
            //todo：目前是实时查的，后面需要直接出报表
            var saleOrderQuery = await _saleOrderDetailRepository.GetQueryableAsync();
            var timeRange = input.GetTimeRange();
            saleOrderQuery = saleOrderQuery
                .Where(a => a.OrderDate >= timeRange.sTime &&
                            a.OrderDate <= timeRange.eTime);
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                saleOrderQuery = saleOrderQuery
                    .Where(a => a.ProductName.Contains(input.KeyWord));
            }

            //销售记录
            var saleOrderDetail = await _saleOrderDetailRepository.ToListAsync(saleOrderQuery);
            if (saleOrderDetail.Any() == false)
            {
                return KdyResult.Success(new QueryPageDto<QueryPageProductSaleReportDto>());
            }

            #region 组合结果
            var productIds = saleOrderDetail
                  .Select(a => a.ProductId)
                  .Distinct()
                  .ToArray();
            //采购记录
            var purchaseOrderDetail =
                await _purchaseOrderDetailRepository.GetPurchaseOrderDetailByProductIdsAsync(productIds);

            //销售产品分组
            var productSaleGroup = saleOrderDetail
                .GroupBy(a => new
                {
                    a.ProductId,
                    a.ProductName
                })
                .Select(a => new
                {
                    a.Key.ProductId,
                    a.Key.ProductName,
                    SaleCount = a.Sum(b => b.Count),
                    AvgSaleUnitPrice = a.Average(b => b.UnitPrice),
                    SumSalePrice = a.Sum(b => b.SumPrice)
                })
                .ToList();
            //采购产品分组
            var productPurchaseGroup = purchaseOrderDetail
                .GroupBy(a => new
                {
                    a.ProductId,
                    a.ProductName
                })
                .Select(a => new
                {
                    a.Key.ProductId,
                    a.Key.ProductName,
                    PurchaseCount = a.Sum(b => b.Count),
                    AvgPurchaseUnitPrice = a.Average(b => b.UnitPrice),
                    SumPurchasePrice = a.Sum(b => b.SumPrice)
                })
                .ToList();

            var tempList = new List<QueryPageProductSaleReportDto>();
            foreach (var item in productSaleGroup)
            {
                var currentPurchaseInfo = productPurchaseGroup
                    .FirstOrDefault(a => a.ProductId == item.ProductId);
                tempList.Add(new QueryPageProductSaleReportDto()
                {
                    ProductName = item.ProductName,
                    PurchaseCount = currentPurchaseInfo?.PurchaseCount,
                    AvgPurchaseUnitPrice = currentPurchaseInfo?.AvgPurchaseUnitPrice,
                    SumPurchasePrice = currentPurchaseInfo?.SumPurchasePrice,

                    SaleCount = item.SaleCount,
                    AvgSaleUnitPrice = item.AvgSaleUnitPrice,
                    SumSalePrice = item.SumSalePrice,
                });
            }
            #endregion

            //分页结果
            var result = tempList
                .Skip((input.Page - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            return KdyResult.Success(new QueryPageDto<QueryPageProductSaleReportDto>()
            {
                Total = tempList.Count,
                Items = result
            });
        }
    }
}
