﻿using AutoMapper;
using Zcy.Dto.Company;
using Zcy.Dto.FinancialMemo;
using Zcy.Dto.Production;
using Zcy.Dto.Products;
using Zcy.Dto.PurchaseSale;
using Zcy.Dto.SysBaseInfo;
using Zcy.Dto.User;
using Zcy.Entity.Company;
using Zcy.Entity.FinancialMemo;
using Zcy.Entity.Production;
using Zcy.Entity.Products;
using Zcy.Entity.PurchaseSale;
using Zcy.Entity.SysBaseInfo;
using Zcy.Entity.User;

namespace Zcy.Dto
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //用户
            CreateMap<SystemUser, QueryPageUserDto>();
            CreateMap<SystemUser, GetCurrentCompanyAllEmployeeDto>();


            //角色菜单
            CreateMap<SystemMenu, QueryPageMenuDto>();
            CreateMap<SystemMenu, GetAllMenuTreeDto>();
            CreateMap<SystemMenu, GetRoleActivateMenuDto>()
                .ForMember(dest => dest.MenuId, target => target.MapFrom(source => source.Id));
            CreateMap<SystemMenu, GetRoleActivateMenuTreeDto>()
                .ForMember(dest => dest.Cacheable, target => target.MapFrom(source => source.IsCache));

            CreateMap<SystemRole, QueryPageRoleDto>();
            CreateMap<SystemRole, GetAllRoleDto>()
                .ForMember(dest => dest.RoleId, target => target.MapFrom(source => source.Id));


            //供应商客户
            CreateMap<SupplierClient, QueryPageSupplierClientDto>();
            CreateMap<SupplierClient, GetValidSupplierClientDto>();
            CreateMap<SystemCompany, CompanyCacheItem>()
                .ForMember(dest => dest.CompanyId, target => target.MapFrom(source => source.Id));
            CreateMap<SystemCompany, QueryPageCompanyDto>();


            //财务备忘录
            CreateMap<IncomeRecord, QueryPageIncomeRecordDto>();
            CreateMap<ProceedsRecord, QueryPageProceedsRecordDto>();


            //产品
            CreateMap<ProductCraft, QueryPageProductCraftDto>();
            CreateMap<ProductCraft, QueryValidProductCraftDto>();
            CreateMap<Product, QueryPageProductDto>();
            CreateMap<Product, QueryValidProductDto>();
            CreateMap<Product, GetProductDetailDto>();
            CreateMap<ProductProcess, ProductProcessItem>()
                .ForMember(dest => dest.ProductProcessId, target => target.MapFrom(source => source.Id));
            CreateMap<ProductType, QueryPageProductTypeDto>();
            CreateMap<ProductType, QueryValidProductTypeDto>();


            //生产
            CreateMap<ReportWork, QueryPageReportWorkDto>();


            //订单
            CreateMap<PurchaseOrder, GetPurchaseOrderDetailDto>();
            CreateMap<PurchaseOrderDetail, GetPurchaseOrderDetailItemDto>();
            CreateMap<PurchaseOrder, QueryPagePurchaseOrderDto>();

            CreateMap<SaleOrder, GetSaleOrderDetailDto>();
            CreateMap<SaleOrderDetail, GetSaleOrderDetailItemDto>();
            CreateMap<SaleOrder, QueryPageSaleOrderDto>();

            CreateMap<ShipmentOrder, GetShipmentOrderDetailDto>();
            CreateMap<ShipmentOrderDetail, GetShipmentOrderDetailItemDto>();
            CreateMap<ShipmentOrder, QueryPageShipmentOrderDto>();

            CreateMap<ReturnOrder, GetReturnOrderDetailDto>();
            CreateMap<ReturnOrderDetail, GetReturnOrderDetailItemDto>();
            CreateMap<ReturnOrder, QueryPageReturnOrderDto>();

            //CreateMap<ActivationCodeTypeV2, QueryPageCodeTypeDto>()
            //    .ForMember(dest => dest.CardTypeName, target => target.MapFrom(source => source.CodeName));
            //CreateMap<ActivationCode, QueryPageActivationCodeDto>();
            //CreateMap<GptWebConfig, QueryPageWebConfigDto>();
        }
    }
}
