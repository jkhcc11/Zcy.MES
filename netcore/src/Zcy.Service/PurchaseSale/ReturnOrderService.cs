﻿using Microsoft.Extensions.Caching.Distributed;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.PurchaseSale;
using Zcy.Entity.Company;
using Zcy.Entity.PurchaseSale;
using Zcy.IRepository.Products;
using Zcy.IRepository.PurchaseSale;
using Zcy.IRepository.User;
using Zcy.IService.Company;
using Zcy.IService.PurchaseSale;
using Zcy.Utility;

namespace Zcy.Service.PurchaseSale
{
    /// <summary>
    /// 退货订单 服务实现
    /// </summary>
    public class ReturnOrderService : BaseOrderService, IReturnOrderService
    {
        private readonly ISystemUserRepository _systemUserRepository;
        private readonly IReturnOrderRepository _returnOrderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBaseRepository<SupplierClient, long> _supplierClientRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly ISystemCompanyService _systemCompanyService;

        public ReturnOrderService(IDistributedCache distributedCache, ISystemCompanyService systemCompanyService,
            ISystemUserRepository systemUserRepository, IProductRepository productRepository,
            IBaseRepository<SupplierClient, long> supplierClientRepository,
            IReturnOrderRepository returnOrderRepository, IBaseRepository<SystemCompany, long> systemCompanyRepository)
        {
            _distributedCache = distributedCache;
            _systemCompanyService = systemCompanyService;
            _systemUserRepository = systemUserRepository;
            _productRepository = productRepository;
            _supplierClientRepository = supplierClientRepository;
            _returnOrderRepository = returnOrderRepository;
            _systemCompanyRepository = systemCompanyRepository;
        }

        /// <summary>
        /// 分页查询退货订单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageReturnOrderDto>>> QueryPageReturnOrderAsync(QueryPageReturnOrderInput input)
        {
            var query = await _returnOrderRepository.GetQueryableAsync();
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);
            query = query.Where(a => a.OrderDate >= timeRange.sTime &&
                                     a.OrderDate <= timeRange.eTime);

            query = query.CreateConditions(input);

            var result = await BaseQueryPageEntityAsync<ReturnOrder, QueryPageReturnOrderDto>(
                _returnOrderRepository
                , query, input);
            if (result.Data.Items.Any())
            {
                await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建退货订单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateReturnOrderAsync(CreateReturnOrderInput input)
        {
            #region 参数检查
            var supplierClient = await _supplierClientRepository
                .FirstOrDefaultAsync(a => a.Id == input.SupplierId &&
                                          a.ClientStatus == PublicStatusEnum.Normal);
            if (supplierClient == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "供应商无效");
            }

            if (input.OrderDetails == null ||
                  input.OrderDetails.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "订单详情不能为空");
            }

            //经办人检测
            var managerUserId = LoginUserInfo.GetUserId();
            var managerUserNick = LoginUserInfo.UserNick ?? LoginUserInfo.GetUserName();
            if (input.ManagerUserId.HasValue)
            {
                var managerUser = await _systemUserRepository.FirstOrDefaultAsync(input.ManagerUserId.Value);
                if (managerUser == null)
                {
                    return KdyResult.Error(KdyResultCode.ParError, "经办人参数错误");
                }

                managerUserId = managerUser.Id;
                managerUserNick = managerUser.UserNick;
            }

            //产品校验
            var productIds = input.OrderDetails.Select(a => a.ProductId).ToArray();
            var dbProductList = await _productRepository.GetValidProductListAsync(productIds);
            var check = ValidOrderDetailProductAsync(dbProductList, input.OrderDetails);
            if (check == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "订单产品异常，有效产品不一致");
            }
            #endregion

            //生成订单号
            var companyCache = await _systemCompanyService.GetCompanyCacheAsync(LoginUserInfo.CompanyId);
            var orderNo = await GeneralFullOrderNoAsync(_distributedCache, _returnOrderRepository,
                companyCache, ReturnOrder.OrderNoPrefix);
            if (string.IsNullOrEmpty(orderNo))
            {
                throw new ZcyCustomException("销售订单生成订单号异常");
            }

            #region 创建订单
            var orderDate = DateTime.Today;
            if (input.OrderDate.HasValue &&
                input.OrderDate <= DateTime.Today)
            {
                orderDate = input.OrderDate.Value;
            }

            //todo:计算价格是这里计算还是重新生成对账单
            var orderEntity = new ReturnOrder(IdGenerateExtension.GenerateId(),
                orderNo,
                managerUserId,
                managerUserNick,
                orderDate)
            {
                ShipmentUser = input.ShipmentUser,
                CompanyId = LoginUserInfo.CompanyId,
                OrderRemark = input.OrderRemark,
                SupplierClientId = supplierClient.Id,
                SupplierClientName = supplierClient.ClientName
            };
            var orderDetail = new List<ReturnOrderDetail>();
            foreach (var inputItem in input.OrderDetails)
            {
                var dbItem = new ReturnOrderDetail(orderEntity.Id, orderDate,
                    inputItem.ProductId, inputItem.Count)
                {
                    Remark = inputItem.Remark,
                    Id = IdGenerateExtension.GenerateId()
                };
                var currentProduct = dbProductList.First(a => a.Id == dbItem.ProductId);
                dbItem.SetProductInfo(currentProduct.ProductName, currentProduct.IsLoose,
                    currentProduct.Unit, currentProduct.Spec, currentProduct.SpecCount);
                orderDetail.Add(dbItem);
            }

            orderEntity.OrderDetails = orderDetail;
            orderEntity.TotalsOrderProductCount();
            await _returnOrderRepository.CreateAsync(orderEntity);
            #endregion

            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(long orderId)
        {
            var entity = await _returnOrderRepository.FirstOrDefaultAsync(orderId);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "订单Id错误");
            }

            await _returnOrderRepository.DeleteAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        ///获取退货订单详情
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetReturnOrderDetailDto>> GetReturnOrderDetailAsync(long orderId)
        {
            var entity = await _returnOrderRepository.GetEntityByIdAsync(orderId);
            var result = BaseMapper.Map<ReturnOrder, GetReturnOrderDetailDto>(entity);
            return KdyResult.Success(result);
        }

        /// <summary>
        ///获取退货订单详情
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetReturnOrderDetailDto>> GetReturnOrderDetailAsync(string orderNo)
        {
            if (OrderNoExt.ValidateOrderNo(orderNo, OrderNoIndexLength) == false)
            {
                return KdyResult.Error<GetReturnOrderDetailDto>(KdyResultCode.ParError, "非有效订单号");
            }

            var entity = await _returnOrderRepository.FirstOrDefaultAsync(orderNo);
            if (entity == null)
            {
                return KdyResult.Error<GetReturnOrderDetailDto>(KdyResultCode.ParError, "订单号无效或不存在");
            }

            var result = BaseMapper.Map<ReturnOrder, GetReturnOrderDetailDto>(entity);
            return KdyResult.Success(result);
        }
    }
}
