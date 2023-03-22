using AutoMapper;
using MediatR;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Commands.User
{
    public class UserUpdateCommand : IRequest<UserDTO>
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, UserDTO>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public UserUpdateCommandHandler(IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            this._mapper = mapper;
            this._identityService = identityService;
            this._currentUserService = currentUserService;
        }

        public async Task<UserDTO> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityService.GetUserAsync(request.Id);

            if (userDetails is null)
            {
                throw new NotFoundException("Invalid User Information");
            }

            var user = _mapper.Map<ApplicationUser>(request);
            var (identityResult, userId) = await _identityService.UpdateUserAsync(user);

            if (!identityResult.Succeeded)
            {
                throw new IdentityResultException(identityResult, "Update User");
            }

            return _mapper.Map<UserDTO>(await _identityService.GetUserAsync(userId));
        }
    }
}
