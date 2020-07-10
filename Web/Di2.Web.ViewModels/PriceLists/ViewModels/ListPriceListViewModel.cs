using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.PriceLists.ViewModels
{
    public class ListPriceListViewModel
    {
        public IEnumerable<PriceListViewModel> PriceLists { get; set; }
    }
}
