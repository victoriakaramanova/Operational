namespace Di2.Web.ViewModels.PriceLists.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.Suppliers.ViewModels;

    public class CreatePriceListInputModel : IMapTo<PriceList> //IMapFrom<Material>, IMapFrom<Supplier>
    {
        // public Material Material { get; set; }
        [Range(1, int.MaxValue)]
        [Display(Name = "Материал")]
        public int MaterialId { get; set; }

        // public Supplier Supplier { get; set; }
        [Range(1, int.MaxValue)]
        [Display(Name = "Доставчик")]
        public int SupplierId { get; set; }

        [Required]
        public double MinimumQuantityPerOrder { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public double CheapRatio => (double)this.UnitPrice / this.MinimumQuantityPerOrder;

        public IEnumerable<MaterialViewModel> Materials { get; set; }

        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}
