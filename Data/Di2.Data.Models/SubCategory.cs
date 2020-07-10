namespace Di2.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Di2.Data.Common.Models;

    public class SubCategory : BaseDeletableModel<int>
    {
        //public SubCategory()
        //{
        //    this.Materials = new HashSet<Material>();
        //}

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        //public virtual Category Category { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        //public virtual ICollection<Material> Materials { get; set; }
        
        public virtual ICollection<SubCategoryMaterial> SubCategoryMaterials { get; set; }
    }
}
