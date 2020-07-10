using AutoMapper;
using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Di2.Web.ViewModels.Categories.ViewModels;

namespace Di2.Web.ViewModels.Deliveries
{
    public class ProductViewModel : IMapFrom<Delivery>, IMapTo<Delivery> //,IMapFrom<CategoryProductsViewModel>,IMapFrom<Category>, IMapFrom<Material>, IMapTo<CategoryProductsViewModel>
    {
        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public double Quantity { get; set; }

        public decimal AvgPrice { get; set; }

        public decimal Cost { get; set; }

        public string Image { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        /*public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Delivery, ProductViewModel>()
                .ForMember(x => x.MaterialName, options =>
                {
                    options.MapFrom(m => m.Material.Name);
                });
            configuration.CreateMap<Delivery, ProductViewModel>()
                .ForMember(x => x.Description, options =>
                  {
                      options.MapFrom(m => m.Material.Description);
                  });
            configuration.CreateMap<Delivery, ProductViewModel>()
                .ForMember(x => x.ExtraInfo, options =>
                {
                    options.MapFrom(m => m.Material.ExtraInfo);
                  });
            configuration.CreateMap<Delivery, ProductViewModel>()
                .ForMember(x => x.Image, options =>
                {
                    options.MapFrom(m => m.Material.Image);
                });
        }*/
    }
}
