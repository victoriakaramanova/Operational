namespace Di2.Web.ViewModels.SubCategories.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Categories.ViewModels;
    using Di2.Web.ViewModels.SubCategories.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class CreateSubCategoryInputModel : IMapFrom<SubCategory>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, SelectListItem>()
                .ForMember(a => a.Text, x => x.MapFrom(z => z.Name));
        }
    }
}
