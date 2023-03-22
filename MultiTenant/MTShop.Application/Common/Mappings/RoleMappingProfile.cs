using AutoMapper;
using MTShop.Application.Commands.Role;
using MTShop.Application.DTOs;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Common.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<ApplicationRole, RoleDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
 
            CreateMap<RoleCreateCommand, ApplicationRole>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(src => src.Id, opt => opt.Ignore())
            .ForMember(src => src.NormalizedName, opt => opt.Ignore())
            .ForMember(src => src.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<RoleUpdateCommand, ApplicationRole>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
