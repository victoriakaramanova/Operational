namespace Di2.Web.ViewModels.OrderSuppliers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.PriceLists.ViewModels;

    public class OrderSuppliersListViewModel : IMapFrom<OrderSupplier>, IMapTo<OrderSupplier>,IMapTo<PriceList>,IMapFrom<PriceList>
    {
        public List<PriceListViewModel> Pricelists { get; set; }

        public List<CreateOrderSupplierInputModel> OrderSub { get; set; }
    }
}
