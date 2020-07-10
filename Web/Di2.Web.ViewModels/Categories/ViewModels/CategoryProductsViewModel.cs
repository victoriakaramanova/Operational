using AutoMapper;
using Di2.Data.Models;
using Di2.Services.Mapping;
using Di2.Web.ViewModels.Deliveries;
using Di2.Web.ViewModels.Orders.ViewModels;
using Di2.Web.ViewModels.OrderSuppliers;
using Di2.Web.ViewModels.SubCategories.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Di2.Web.ViewModels.Categories.ViewModels
{
    public class CategoryProductsViewModel : IMapFrom<Delivery>, IMapTo<CategoryProductsViewModel>,IMapTo<Delivery>, IMapTo<OrderViewModel>,IMapFrom<Material>
    {
        public int MaterialId { get; set; }

        [Display(Name = "Продукт")]
        public string MaterialName { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Допълнителна информация")]
        public string ExtraInfo { get; set; }

        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Display(Name = "Доставна цена")] // ONLY FOR ADMIN USAGE!!!
        public decimal UnitPrice { get; set; }
        
        [Display(Name = "Единична цена")]
        public decimal AvgPrice { get; set; }

        public string Image { get; set; }

        public int CategoryId { get; set; }

        public decimal Cost { get; set; }

        [Display(Name = "Подкатегория на продукта")]
        public string SubCategoryName { get; set; }

        public int SubCategoryId { get; set; }

        public ICollection<IFormFile> PicturesFormFiles { get; set; }
    }
}
