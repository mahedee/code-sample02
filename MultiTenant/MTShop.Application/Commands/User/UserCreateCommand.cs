using AutoMapper;
using MediatR;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Commands.User
{
    public class UserCreateCommand : IRequest<UserDTO>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        //public string TenantId { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UserCreateCommandHanlder : IRequestHandler<UserCreateCommand, UserDTO>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public UserCreateCommandHanlder(IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<UserDTO> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<ApplicationUser>(request);
            user.TenantId = _currentUserService.TenantId;
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;

            var (identityResult, userId) = await _identityService.CreateUserAsync(user, request.Password);

            if (!identityResult.Succeeded)
                throw new IdentityResultException(identityResult, "User Creation Failed!");

            return _mapper.Map<UserDTO>(await _identityService.GetUserAsync(userId));

        }
    }
}
