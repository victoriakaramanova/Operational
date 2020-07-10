using Di2.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class Receipt : BaseDeletableModel<string>
    {
        public Receipt()
        {
            this.Orders = new HashSet<Order>();
        }

        public DateTime IssuedOn { get; set; }

        public ICollection<Order> Orders { get; set; }

        public string RecipientId { get; set; }

        public ApplicationUser Recipient { get; set; }

        public string DeliveryAddress { get; set; }
    }
}
