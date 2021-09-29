using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CatalogResponse
    {
        public int OrderId { get; set; }
        public int CatalogId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
