using Di2.Data.Models;
using Di2.Services.Mapping;
using Di2.Web.ViewModels.Suppliers.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.PriceLists.ViewModels
{
    public class SuppliersPerMaterialViewModel : IMapFrom<PriceList>
    {
        public string Material { get; set; }

        public double MinimumQuantityPerOrder { get; set; }

        public decimal UnitPrice { get; set; }

        public IEnumerable<SuppliersViewModel> Suppliers { get; set; }

    }
}
