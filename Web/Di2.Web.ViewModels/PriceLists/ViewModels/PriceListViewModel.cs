using Di2.Data.Models;
using Di2.Services.Mapping;
using Di2.Web.ViewModels.Materials.ViewModels;
using Di2.Web.ViewModels.Suppliers.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Di2.Web.ViewModels.PriceLists.ViewModels
{
    public class PriceListViewModel : IMapFrom<PriceList>, IMapTo<PriceList>
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public Material Material { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public double MinimumQuantityPerOrder { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
