using Di2.Data.Common.Models;
using Di2.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Data.Models
{
    public class Order : BaseDeletableModel<string>
    {
        public int MaterialId { get; set; }

        public Material Material { get; set; }

        public string MaterialName { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public string Image { get; set; }

        public string SubCategoryName { get; set; }

        public DateTime IssuedOn { get; set; }

        public double Quantity { get; set; }

        public decimal AvgPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public string OrdererId { get; set; }

        public ApplicationUser Orderer { get; set; }

        public string DeliveryAddress { get; set; }

        public int StatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string ReceiptId { get; set; }

        public virtual Receipt Receipt { get; set; }
    }
}
