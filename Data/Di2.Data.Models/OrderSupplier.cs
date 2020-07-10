using Di2.Data.Common.Models;
using Di2.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class OrderSupplier : BaseDeletableModel<int>
    {
        public DateTime OrderDate { get; set; }

        public int MaterialId { get; set; }

        public virtual Material Material { get; set; }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public decimal UnitPrice { get; set; }

        public double Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public OrderStatus Status { get; set; }
    }
}
