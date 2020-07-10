using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.Pictures
{
    public class PictureViewModel : IMapFrom<Picture>
    {
        public string Url { get; set; }
    }
}
