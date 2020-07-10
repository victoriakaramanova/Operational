using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.SubCategories.ViewModels
{
    public class SubCategoryViewModel : IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
