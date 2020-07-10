using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.Orders.ViewModels
{
    public class ReceiptViewModel : IMapFrom<Order>, IMapFrom<Receipt>
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public IEnumerable<OrderViewModel> Orders { get; set; }

        public string RecipientId { get; set; }

        public string RecipientName { get; set; }

        public ApplicationUser Recipient { get; set; }

        public string DeliveryAddress { get; set; }
    }
}
