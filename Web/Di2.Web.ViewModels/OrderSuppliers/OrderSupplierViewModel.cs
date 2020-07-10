namespace Di2.Web.ViewModels.OrderSuppliers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Data.Models.Enums;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.PriceLists.ViewModels;

    public class OrderSupplierViewModel : IMapFrom<OrderSupplier>, IMapFrom<PriceList>
    {
        // public PriceListViewModel PriceList { get; set; }
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public int MaterialId { get; set; }

        public Material Material { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public double MinimumQuantityPerOrder { get; set; }

        public double Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        //public string UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public int StatusId { get; set; }

        public string SwitchValue => (int)this.Status == 1 ?
         "v-switcher v-switcher-mode--tri v-switcher-state--active"
         : (int)this.Status == -1
         ? "v-switcher v-switcher-mode--tri v-switcher-state--passive"
         : "v-switcher v-switcher-mode--tri v-switcher-state--neutral";

        public OrderStatus Status { get; set; }
    }
}
