using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.PriceLists.ViewModels
{
    public class SingleViewModel : IMapFrom<PriceList>
    {
        public string Material { get; set; }

        public string Supplier { get; set; }

        public double MinimumQuantityPerOrder { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
