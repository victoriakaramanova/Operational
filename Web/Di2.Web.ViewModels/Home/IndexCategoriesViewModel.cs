namespace Di2.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;

    public class IndexCategoriesViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string NameEng { get; set; }

        public string Url => $"/{this.NameEng}";
    }
}
