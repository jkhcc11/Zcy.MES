using AutoMapper;
using Zcy.BaseInterface;
using Zcy.Dto.Company;
using Zcy.Dto.FinancialMemo;
using Zcy.Dto.Production;
using Zcy.Dto.Production.Exports;
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
            CreateMap<SystemUser, GetCurrentCompanyValidEmployeeDto>();


            //角色菜单
            CreateMap<SystemMenu, QueryPageMenuDto>();
            CreateMap<SystemMenu, GetAllMenuTreeDto>();
            CreateMap<SystemMenu, GetRoleActivateMenuDto>()
                .ForMember(dest => dest.MenuId, target => target.MapFrom(source => source.Id));
            CreateMap<SystemMenu, GetRoleActivateMenuTreeDto>()
                .ForMember(dest => dest.Cacheable, target => target.MapFrom(source => source.IsCache))
                .ForMember(dest => dest.Hidden, target => target.MapFrom(source => source.IsHidden));

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
            CreateMap<IncomeRecord, QueryPageIncomeRecordDto>()
                .ForMember(dest => dest.RecordDate,
                    target => target.MapFrom(source => source.RecordDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<ProceedsRecord, QueryPageProceedsRecordDto>()
                .ForMember(dest => dest.RecordDate,
                    target => target.MapFrom(source => source.RecordDate.ToString(ZcyMesConst.DateFormat)));


            //产品
            CreateMap<ProductCraft, QueryPageProductCraftDto>();
            CreateMap<ProductCraft, QueryValidProductCraftDto>();
            CreateMap<ProductCraft, ProductCraftItem>()
                .ForMember(dest => dest.CraftId, target => target.MapFrom(source => source.Id))
                .ForMember(dest => dest.CraftPrice, target => target.MapFrom(source => source.UnitPrice));
            CreateMap<Product, QueryPageProductDto>();
            CreateMap<Product, QueryValidProductDto>();
            CreateMap<Product, GetProductDetailDto>();
            CreateMap<ProductProcess, ProductProcessItem>()
                .ForMember(dest => dest.ProductProcessId, target => target.MapFrom(source => source.Id));
            CreateMap<ProductType, QueryPageProductTypeDto>();
            CreateMap<ProductType, QueryValidProductTypeDto>();
            CreateMap<Product, GetProductDetailCascadeDto>();
            CreateMap<ProductProcess, GetProductDetailCascadeItem>()
                .ForMember(dest => dest.ProductProcessId, target => target.MapFrom(source => source.Id));
            CreateMap<ProductCraft, ProductDetailCascadeCraftItem>()
                .ForMember(dest => dest.CraftId, target => target.MapFrom(source => source.Id));
            CreateMap<Product, QueryValidProductWithNormalDto>()
                .ForMember(dest => dest.ProductId, target => target.MapFrom(source => source.Id));
            CreateMap<ProductProcess, GetProductProcessWithNormalDto>()
                .ForMember(dest => dest.ProductProcessId, target => target.MapFrom(source => source.Id))
                .ForMember(dest => dest.CraftName,
                    target => target.MapFrom(source => source.ProductCraft == null ? "" : source.ProductCraft.CraftName))
                .ForMember(dest => dest.BillingType,
                    target => target.MapFrom(source => source.ProductCraft == null ? default : source.ProductCraft.BillingType))
                .ForMember(dest => dest.OrderBy,
                    target => target.MapFrom(source => source.OrderBy));


            //生产
            CreateMap<ReportWork, QueryPageReportWorkDto>()
                .ForMember(dest => dest.ReportWorkDate,
                    target => target.MapFrom(source => source.ReportWorkDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<ReportWork, QueryPageReportWorkForAdminDto>()
                .ForMember(dest => dest.ReportWorkDate,
                    target => target.MapFrom(source => source.ReportWorkDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<ReportWork, ExportDayReportWorkDto>()
                .ForMember(dest => dest.ReportWorkDate,
                    target => target.MapFrom(source => source.ReportWorkDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<ReportWork, ExportReportWorkWithProductDto>()
                .ForMember(dest => dest.ReportWorkDate,
                    target => target.MapFrom(source => source.ReportWorkDate.ToString(ZcyMesConst.DateFormat)));


            //订单
            CreateMap<PurchaseOrder, GetPurchaseOrderDetailDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<PurchaseOrderDetail, GetPurchaseOrderDetailItemDto>();
            CreateMap<PurchaseOrder, QueryPagePurchaseOrderDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));

            CreateMap<SaleOrder, GetSaleOrderDetailDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<SaleOrderDetail, GetSaleOrderDetailItemDto>();
            CreateMap<SaleOrder, QueryPageSaleOrderDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));

            CreateMap<ShipmentOrder, GetShipmentOrderDetailDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<ShipmentOrderDetail, GetShipmentOrderDetailItemDto>();
            CreateMap<ShipmentOrder, QueryPageShipmentOrderDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));

            CreateMap<ReturnOrder, GetReturnOrderDetailDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));
            CreateMap<ReturnOrderDetail, GetReturnOrderDetailItemDto>();
            CreateMap<ReturnOrder, QueryPageReturnOrderDto>()
                .ForMember(dest => dest.OrderDate,
                    target => target.MapFrom(source => source.OrderDate.ToString(ZcyMesConst.DateFormat)));

            //CreateMap<ActivationCodeTypeV2, QueryPageCodeTypeDto>()
            //    .ForMember(dest => dest.CardTypeName, target => target.MapFrom(source => source.CodeName));
            //CreateMap<ActivationCode, QueryPageActivationCodeDto>();
            //CreateMap<GptWebConfig, QueryPageWebConfigDto>();
        }
    }
}
