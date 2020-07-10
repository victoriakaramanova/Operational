namespace Di2.Web.ViewModels.Categories.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class CreateCategoryInputModel : IMapFrom<Category>, IMapTo<Category>
    {
        public string Name { get; set; }

        public string NameEng { get; set; }
    }
}
