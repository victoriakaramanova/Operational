using Di2.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class Picture : BaseDeletableModel<string>
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
