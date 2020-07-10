using Di2.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class SubCategoryMaterial: BaseDeletableModel<int>
    {
        public int? SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

        public int? MaterialId { get; set; }

        public Material Material { get; set; }
    }
}
