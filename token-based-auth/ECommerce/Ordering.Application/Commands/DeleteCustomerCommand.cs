using MediatR;
using System;

namespace Ordering.Application.Commands
{
    // Customer create command with string response
    public class DeleteCustomerCommand : IRequest<String>
    {
        public Int64 Id { get; private set; }

        public DeleteCustomerCommand(Int64 Id)
        {
            this.Id = Id;
        }
    }
}
