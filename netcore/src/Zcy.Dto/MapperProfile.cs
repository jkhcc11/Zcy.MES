﻿using AutoMapper;
using Zcy.Dto.Company;
using Zcy.Dto.FinancialMemo;
using Zcy.Dto.Products;
using Zcy.Dto.SysBaseInfo;
using Zcy.Dto.User;
using Zcy.Entity.Company;
using Zcy.Entity.FinancialMemo;
using Zcy.Entity.Products;
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
            CreateMap<SystemCompany, CompanyCacheItem>();
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
            CreateMap<ProductProcess, ProductProcessItem>();
            CreateMap<ProductType, QueryPageProductTypeDto>();
            CreateMap<ProductType, QueryValidProductTypeDto>();


            //CreateMap<ActivationCodeTypeV2, QueryPageCodeTypeDto>()
            //    .ForMember(dest => dest.CardTypeName, target => target.MapFrom(source => source.CodeName));
            //CreateMap<ActivationCode, QueryPageActivationCodeDto>();
            //CreateMap<GptWebConfig, QueryPageWebConfigDto>();
        }
    }
}
