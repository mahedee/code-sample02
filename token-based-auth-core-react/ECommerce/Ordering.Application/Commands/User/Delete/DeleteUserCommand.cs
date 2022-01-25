using MediatR;
using Ordering.Application.Common.Interfaces;

namespace Ordering.Application.Commands.User.Delete
{
    public class DeleteUserCommand : IRequest<int>
    {
        public string Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IIdentityService _identityService;

        public DeleteUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.DeleteUserAsync(request.Id);

            return result ? 1 : 0;
        }
    }
}
