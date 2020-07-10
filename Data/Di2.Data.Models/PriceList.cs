namespace Di2.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Common.Models;

    public class PriceList : BaseDeletableModel<int>
    {
        public PriceList()
        {
            this.OrderSuppliers = new HashSet<OrderSupplier>();
        }

        public int MaterialId { get; set; }

        public virtual Material Material { get; set; }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public double MinimumQuantityPerOrder { get; set; }

        public decimal UnitPrice { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public double CheapRatio { get; set; }

        public IEnumerable<OrderSupplier> OrderSuppliers { get; set; }
    }
}
