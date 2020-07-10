using Di2.Data.Models;
using Di2.Data.Models.Enums;
using Di2.Services.Mapping;
using Di2.Web.ViewModels.Materials.ViewModels;
using Di2.Web.ViewModels.Orders.InputModels;
using Di2.Web.ViewModels.Pictures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Di2.Web.ViewModels.Orders.ViewModels
{
    public class OrderViewModel:IMapFrom<Order>,IMapTo<Order>, IMapTo<OrderInputModel>
    {
        public string Id { get; set; }

        public int MaterialId { get; set; }

        //public MaterialsViewModel Material { get; set; }
        public string MaterialName { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public string Image { get; set; }

        public string SubCategoryName { get; set; }

        public DateTime IssuedOn { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Въведете положително число!")]
        public double Quantity { get; set; }

        public decimal AvgPrice { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public string OrdererId { get; set; }

        public ApplicationUser Orderer { get; set; }

        public int StatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string ReceiptId { get; set; }

        public Receipt Receipt { get; set; }

        public ICollection<PictureViewModel> Pictures { get; set; }
    }
}
