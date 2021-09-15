using MediatR;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Queries
{

    public class GetCustomerByEmailQuery: IRequest<Customer>
    {
        public string Email { get; private set; }
        
        public GetCustomerByEmailQuery(string email)
        {
            this.Email = email;
        }

    }
}
