using Di2.Data.Common.Models;
using Di2.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class SupplyOrderStatus : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public virtual OrderSupplier OrderSupplier { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
