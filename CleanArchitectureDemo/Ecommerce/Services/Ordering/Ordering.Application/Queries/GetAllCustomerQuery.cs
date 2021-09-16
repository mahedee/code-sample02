using MediatR;
using Ordering.Core.Entities;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    public record GetAllCustomerQuery : IRequest<List<Customer>>
    {

    }
}
