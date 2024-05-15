using AutoMapper;
using Zcy.Dto.SysBaseInfo;
using Zcy.Dto.User;
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

            CreateMap<SystemMenu, QueryPageRoleDto>();
            CreateMap<SystemRole, GetAllRoleDto>()
                .ForMember(dest => dest.RoleId, target => target.MapFrom(source => source.Id));
            CreateMap<SystemMenu, GetRoleActivateMenuDto>()
                .ForMember(dest => dest.MenuId, target => target.MapFrom(source => source.Id));
            CreateMap<SystemMenu, GetRoleActivateMenuTreeDto>()
                .ForMember(dest => dest.Cacheable, target => target.MapFrom(source => source.IsCache));


            //CreateMap<ActivationCodeTypeV2, QueryPageCodeTypeDto>()
            //    .ForMember(dest => dest.CardTypeName, target => target.MapFrom(source => source.CodeName));
            //CreateMap<ActivationCode, QueryPageActivationCodeDto>();
            //CreateMap<GptWebConfig, QueryPageWebConfigDto>();
        }
    }
}
