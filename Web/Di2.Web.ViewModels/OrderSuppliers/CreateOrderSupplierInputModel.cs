namespace Di2.Web.ViewModels.OrderSuppliers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.PriceLists.ViewModels;

    public class CreateOrderSupplierInputModel : IMapFrom<OrderSupplier>, IMapTo<OrderSupplier>
    {
        public DateTime OrderDate { get; set; }

        public double Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
