namespace Di2.Web.ViewModels.Materials.InputModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Categories.ViewModels;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.SubCategories.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    [Authorize]
    public class CreateMaterialInputModel : IMapFrom<Material>, IMapTo<Material>
    {
        // public IList<SubCategoriesViewModel> SubCategoriesViewModelList { get; set; }

        // public SubCategoriesViewModel SubCategoriesViewModel { get; set; }

        [Required]
        public string Name { get; set; }

        // [DataType(DataType.Date)] - for short date format
        [Required]
        public string Description { get; set; }

        [Display(Name = "Допълнителна информация")]
        public string ExtraInfo { get; set; }

        [Display(Name = "Категория")]
        [Required]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Display(Name = "Подкатегория")]
        [Required]
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; }
    }
}
