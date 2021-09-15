using MediatR;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Queries
{
    public record GetAllCustomerQuery : IRequest<List<Customer>>
    {

    }
}
