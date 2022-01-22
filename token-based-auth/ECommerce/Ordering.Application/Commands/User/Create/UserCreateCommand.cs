using MediatR;

namespace Ordering.Application.Commands.User.Create
{
    public class UserCreateCommand : IRequest<int>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public List<string> Roles { get; set; }
    }
}
