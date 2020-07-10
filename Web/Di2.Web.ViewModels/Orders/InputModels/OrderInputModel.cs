using Di2.Data.Common.Repositories;
using Di2.Data.Models;
using Di2.Services.Mapping;
using Di2.Web.ViewModels.Orders.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Di2.Web.ViewModels.Orders.InputModels
{
    public class OrderInputModel: IMapTo<OrderViewModel>,IMapFrom<Order>, IMapFrom<Delivery>
    {
        [Required]
        public int MaterialId { get; set; }

        [Remote(action:"CheckDeliveredQty",controller:"Deliveries",AdditionalFields ="materialId",ErrorMessage ="Order less qty",HttpMethod ="post")]
        [Range(1,int.MaxValue, ErrorMessage = "Въведете положително число!")]
        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        public string MaterialName { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public string Image { get; set; }

        public string SubCategoryName { get; set; }

        public DateTime IssuedOn { get; set; }

        public decimal AvgPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string OrdererId { get; set; }

        public int StatusId { get; set; }

        public string DeliveryAddress { get; set; }

        //public OrderStatus OrderStatus { get; set; }

        public List<IFormFile> PicturesFormFiles { get; set; }
    }

   // [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IsLessThanDeliveredQty:ValidationAttribute
    {
        //private readonly int materialId;
        //private readonly double quantity;
       // private readonly IDeletableEntityRepository<Delivery> deliveriesRepository;

       /* public IsLessThanDeliveredQty(int materialId, double quantity ,IDeletableEntityRepository<Delivery> deliveriesRepository)
        {
            this.materialId = materialId;
            this.quantity = quantity;
            this.deliveriesRepository = deliveriesRepository;
        }*/

        /*protected override bool IsValid(object value, ValidationContext validationContext)
        {
            //var deliveredQuantity = this.deliveriesRepository.All()
            //    .Where(x => x.MaterialId == this.materialId).FirstOrDefault().Quantity;
            Object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            Object 
            
            if (deliveredQuantity<this.quantity)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }*/
    }
}
