using Di2.Common;
using Di2.Data.Common.Repositories;
using Di2.Data.Models;
using Di2.Data.Models.Enums;
using Di2.Services.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public class TotalsCustomerService : ITotalsCustomerService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IOrderService orderService;
        private readonly IEmailSender sender;
        private readonly IDeletableEntityRepository<Delivery> deliveriesRepository;

        //private readonly UserManager<ApplicationUser> userManager;

        public TotalsCustomerService(
            IDeletableEntityRepository<Order> ordersRepository,
            IOrderService orderService,
            IEmailSender sender,
            IDeletableEntityRepository<Delivery> deliveriesRepository)
        {
            this.ordersRepository = ordersRepository;
            this.orderService = orderService;
            this.sender = sender;
            this.deliveriesRepository = deliveriesRepository;
            //this.userManager = userManager;
        }

        public async Task<int> ChangeOrderStatus(string orderId, int isCompleted, ApplicationUser orderer)
        {
            var order = this.ordersRepository.All()
                .FirstOrDefault(x => x.Id == orderId);
            // StringBuilder sb = new StringBuilder();
            // var user = this.ordersRepository.All().FirstOrDefault(x=>x.Orderer.Email==ordererEmail)

            if (isCompleted == 0)
            {
                order.StatusId = (int)OrderStatus.Completed;
                await this.sender.SendEmailAsync(GlobalConstants.SystemEmail, GlobalConstants.SystemName, orderer.Email, $"Готова поръчка по разписка " + order.ReceiptId, $"Здравейте, {orderer.UserName}, {Environment.NewLine}Поръчката Ви {order.Id}/{order.IssuedOn} на {order.MaterialName}, {order.Quantity} броя за {order.TotalPrice.ToString("f2")} лв с доставка на адрес: {order.DeliveryAddress} е готова.{Environment.NewLine}Поздрави, {GlobalConstants.SystemName}");
            }
            else
                if (isCompleted == 1)
            {
                order.StatusId = (int)OrderStatus.Sent;
            }
            else
            {
                order.StatusId = (int)OrderStatus.Canceled;
                await this.sender.SendEmailAsync(GlobalConstants.SystemEmail, GlobalConstants.SystemName, orderer.Email, $"Отказ на Ваша поръчка по разписка " + order.ReceiptId, $"Здравейте, {orderer.UserName}, Поръчката Ви {order.Id}/{order.IssuedOn} на {order.MaterialName}, {order.Quantity} за {order.TotalPrice.ToString("f2")} лв е отказана. Моля потърсете ни за подробности. Поздрави, {GlobalConstants.SystemName}");
            }

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            return order.StatusId;
        }

        public int GetStatus(string orderId)
        {
            var status = this.ordersRepository.All()
                .FirstOrDefault(x => x.Id == orderId);
            var s = Enum.Parse(typeof(OrderStatus), status.ToString()) as Enum;
            return Convert.ToInt32(s);
        }

        public bool IsAvailableQtyEnough(Order order)
        {
            var availableQty = this.deliveriesRepository.All()
                .Where(x => x.MaterialId == order.MaterialId)
                .Sum(x => x.RemainingQuantity);
            if (order.Quantity <= availableQty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<double> DecreaseDeliveriesAsync(Order order)
        {
            var availableQty = this.deliveriesRepository.All()
                .Where(x => x.MaterialId == order.MaterialId)
                .Sum(x => x.RemainingQuantity);
            double interimQty = order.Quantity;
            foreach (var item in this.deliveriesRepository.All()
                    .Where(x => x.MaterialId == order.MaterialId && x.RemainingQuantity > 0).OrderBy(x => x.CreatedOn))
            {
                if (interimQty > item.RemainingQuantity)
                {
                    interimQty -= item.RemainingQuantity;
                    item.RemainingQuantity = 0;
                    this.deliveriesRepository.Delete(item);
                    // order.Quantity -= item.RemainingQuantity;
                    this.deliveriesRepository.Update(item);
                }
                else
                {
                    item.RemainingQuantity -= interimQty;
                    if (item.RemainingQuantity == 0)
                    {
                        //item.IsDeleted = true;
                        this.deliveriesRepository.Delete(item);
                    }

                        interimQty = 0;
                        this.deliveriesRepository.Update(item);
                    //await this.deliveriesRepository.SaveChangesAsync();
                }
            }

            await this.deliveriesRepository.SaveChangesAsync();
            return this.deliveriesRepository.All()
                .Where(x => x.MaterialId == order.MaterialId)
                .Sum(x => x.RemainingQuantity);
        }
    }
}
