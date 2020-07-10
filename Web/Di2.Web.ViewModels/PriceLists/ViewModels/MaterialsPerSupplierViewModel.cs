namespace Di2.Web.ViewModels.PriceLists.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Materials.ViewModels;

    public class MaterialsPerSupplierViewModel : IMapFrom<PriceList>
    {
        public string Supplier { get; set; }

        public double MinimumQuantityPerOrder { get; set; }

        public decimal UnitPrice { get; set; }

        public IEnumerable<MaterialsViewModel> Materials { get; set; }
    }
}
