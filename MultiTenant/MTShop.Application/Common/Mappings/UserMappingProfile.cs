using AutoMapper;
using MTShop.Application.Commands.User;
using MTShop.Application.DTOs;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Common.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserCreateCommand, ApplicationUser>();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<UserUpdateCommand, ApplicationUser>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
