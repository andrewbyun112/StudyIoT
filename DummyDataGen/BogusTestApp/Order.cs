using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogusTestApp
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Decimal OrderValue { get; set; }
        public bool Shipped { get; set; }
    }
}
