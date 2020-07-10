using Di2.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class Delivery : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public virtual OrderSupplier OrderSupplier { get; set; }

        public int MaterialId { get; set; }

        public virtual Material Material { get; set; }

        public string MaterialName { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public int CategoryId { get; set; }

        //public virtual Category Category { get; set; }

        public int SubCategoryId { get; set; }

        //public virtual SubCategory SubCategory { get; set; }

        public string Image { get; set; }

        // public double QuantityOnStock { get; set; }

        public double Quantity { get; set; }

        public decimal SupplierUnitPrice { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Cost { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public double RemainingQuantity { get; set; }
    }
}
