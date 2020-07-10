namespace Di2.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using Di2.Data.Common.Models;
    using Microsoft.AspNetCore.Http;

    public class Material : BaseDeletableModel<int>
    {
        //public Material()
        //{
        //    this.PriceLists = new HashSet<PriceList>();
            
        //}

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int? SubCategoryId { get; set; }

        //public virtual SubCategory SubCategory { get; set; }

        public string Image { get; set; }

       public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<PriceList> PriceLists { get; set; }

        public virtual ICollection<SubCategoryMaterial> SubCategoryMaterials { get; set; }

        public virtual ICollection<OrderSupplier> OrderSuppliers { get; }

        public virtual ICollection<Delivery> Deliveries { get; }

    }
}
