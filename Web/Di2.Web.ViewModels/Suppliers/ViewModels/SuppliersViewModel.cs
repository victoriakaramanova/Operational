namespace Di2.Web.ViewModels.Suppliers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;

    public class SuppliersViewModel : IMapFrom<Supplier>
    {
        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}
