using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.Orders.ViewModels
{
    public class OrdersViewModel : IMapFrom<Order>
    {
        public string DeliveryAddress { get; set; }

        public List<OrderViewModel> Orders { get; set; }
    }
}
