namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    using Di2.Common;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Data.Models.Enums;
    using Di2.Services.Mapping;
    using Di2.Services.Messaging;
    using Di2.Web.ViewModels.Orders.InputModels;
    using Di2.Web.ViewModels.Orders.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IDeletableEntityRepository<Delivery> deliveriesRepository;
        private readonly IDeletableEntityRepository<Material> materialsRepository;
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;
        private readonly IDeletableEntityRepository<Receipt> receiptsRepository;
        private readonly IEmailSender sender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPictureService pictureService;

        public OrderService(
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<Delivery> deliveriesRepository,
            IDeletableEntityRepository<Material> materialsRepository,
            IDeletableEntityRepository<SubCategory> subCategoriesRepository,
            IDeletableEntityRepository<Receipt> receiptsRepository,
            IEmailSender sender,
            UserManager<ApplicationUser> userManager,
            IPictureService pictureService)
        {
            this.ordersRepository = ordersRepository;
            this.deliveriesRepository = deliveriesRepository;
            this.materialsRepository = materialsRepository;
            this.subCategoriesRepository = subCategoriesRepository;
            this.receiptsRepository = receiptsRepository;
            this.sender = sender;
            this.userManager = userManager;
            this.pictureService = pictureService;
        }

        public async Task<int> CreateOrder(OrderInputModel input, string userId, List<string> customerImages = null)
        {
            var image = this.materialsRepository.All()
                .Where(x => x.Id == input.MaterialId)
                .Select(x => x.Image)
                .FirstOrDefault();
            var material = this.materialsRepository.All()
                .FirstOrDefault(x => x.Id == input.MaterialId);

            // var subCategoryId = this.materialsRepository.All()
            //.Select(x => x.SubCategoryId).FirstOrDefault();
            // var subCategoryName = this.subCategoriesRepository.All()
            //    .Where(x => x.Name == input.SubCategoryName).FirstOrDefault();
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                Material = material,
                MaterialId = input.MaterialId,
                MaterialName = input.MaterialName,
                Description = input.Description,
                ExtraInfo = input.ExtraInfo,
                Image = image,
                SubCategoryName = input.SubCategoryName,
                Quantity = input.Quantity,
                AvgPrice = input.AvgPrice,
                StatusId = (int)OrderStatus.Created,
                IssuedOn = DateTime.UtcNow,
                TotalPrice = input.AvgPrice * (decimal)input.Quantity,
                OrdererId = userId,
                //DeliveryAddresss = input.DeliveryAddress,
                //CustomerImages = customerImages,
            };
            //order.OrderStatus = OrderStatus.Sent;
            await this.ordersRepository.AddAsync(order);
            if (material.CategoryId == 1 && input.PicturesFormFiles!=null)
            {
                await this.pictureService.Upload(input.PicturesFormFiles, order.Id);
                /*if (order.Pictures.Count > 0)
                {
                    order.AvgPrice = input.AvgPrice * 1.12m;
                    order.TotalPrice = input.AvgPrice * (decimal)input.Quantity;
                    this.ordersRepository.Update(order);
                }*/
            }

            int result =
                await this.ordersRepository.SaveChangesAsync();
            return result;
        }

        public List<T> GetAll<T>()
        {
            IQueryable<Order> query = this.ordersRepository.All();
            query.Select(x => x.ModifiedOn);
            return query.To<T>().ToList();
        }

        public async Task UpdateOrder(OrdersViewModel input)
        {
            Order dbOrder;
            double inputQty;
            foreach (var order in input.Orders)
            {
                dbOrder = this.ordersRepository.All()
                    .Where(x => x.Id == order.Id).FirstOrDefault();
                inputQty = order.Quantity;
                if (dbOrder.Quantity != inputQty)
                {
                    dbOrder.Quantity = inputQty;
                    dbOrder.TotalPrice = (decimal)order.Quantity * dbOrder.AvgPrice;
                    this.ordersRepository.Update(dbOrder);
                    await this.ordersRepository.SaveChangesAsync();
                }
            }
        }

        public async Task CompleteOrder(OrdersViewModel input)
        {
            Order dbOrder;
            foreach (var order in input.Orders)
            {
                dbOrder = this.ordersRepository.All()
                    .Where(x => x.Id == order.Id).FirstOrDefault();
                if (dbOrder == null || dbOrder.StatusId != (int)OrderStatus.Created)
                {
                    throw new ArgumentException(nameof(dbOrder));
                }

                dbOrder.DeliveryAddress = input.DeliveryAddress == null ? dbOrder.Orderer.Address : input.DeliveryAddress;
                dbOrder.StatusId = (int)OrderStatus.Sent;
                this.ordersRepository.Update(dbOrder);
                await this.ordersRepository.SaveChangesAsync();
            }
        }

        public async Task<string> CreateReceipt(string recipientId,string deliveryAddress)
        {
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                IssuedOn = DateTime.UtcNow,
                RecipientId = recipientId,
                DeliveryAddress = deliveryAddress,
            };
            await this.receiptsRepository.AddAsync(receipt);
            await this.receiptsRepository.SaveChangesAsync();
            return receipt.Id;
        }

        public async Task<int> AssignReceiptToOrders(string receiptId)
        {
            var receipt = this.receiptsRepository.All()
                .Where(x => x.Id == receiptId).FirstOrDefault();
            var orders = this.ordersRepository.All()
                .Where(x => x.OrdererId == receipt.RecipientId)
                .Where(x => x.StatusId == (int)OrderStatus.Sent)
                .Where(x => x.ReceiptId == null) //NB!!!
                .ToList();
            foreach (var order in orders)
            {
                order.ReceiptId = receiptId;
                this.ordersRepository.Update(order);
                await this.ordersRepository.SaveChangesAsync();
            }

            var result = await this.ordersRepository.SaveChangesAsync();
            return result;
        }

        public IEnumerable<T> GetReceiptOrders<T>(string receiptId)
        {
            Receipt receipt = this.receiptsRepository.All()
                .Where(x => x.Id == receiptId).FirstOrDefault();
            IQueryable<Order> query = this.ordersRepository.All()
                .Where(x => x.OrdererId == receipt.RecipientId);
            query = query.Where(x => x.StatusId == (int)OrderStatus.Sent);
            //var elapsedTime = DateTime.UtcNow.Subtract(receipt.IssuedOn);
            //query = query.Where(elapsedTime<TimeSpan.FromSeconds(3));
            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetReceiptOrdersByReceiptId<T>(string receiptId,string userId)
        {
            Receipt receipt = this.receiptsRepository.All()
               .Where(x => x.Id == receiptId).FirstOrDefault();
            IQueryable<Order> query = this.ordersRepository.All()
                .Where(x => x.Receipt.RecipientId == userId);
            query = query.Where(x => x.StatusId == (int)OrderStatus.Sent);
            return query.To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            var receipt = this.ordersRepository.All()
                .Where(x => x.ReceiptId == id)
                .To<T>().FirstOrDefault();
            return receipt;
        }

        public string GetRecipientName(string receiptId)
        {
            var recipient = this.receiptsRepository.All()
                .Where(x => x.Id == receiptId).FirstOrDefault().Recipient;
            return recipient.UserName;
        }

        public int GetCount()
        {
            return this.ordersRepository.All()
                .Where(x => x.StatusId == (int)OrderStatus.Sent)
                .Count();
        }

        public async Task<string> DeleteAsync(string id)
        {
            var order = this.ordersRepository.All().FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return null;
            }

            /*var orderMaterialCategoryId = this.ordersRepository.All()
                    .FirstOrDefault(x => x.Id == id).Material.CategoryId;
            this.ordersRepository.All()
                    .FirstOrDefault(x => x.Id == id)
                    .Material.CategoryId = null;
            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();*/
            //orderMaterialCategoryId == null;
            //var material = this.materialsRepository.All().FirstOrDefault(x => x.Id == orderMatId);
            //material.CategoryId
            this.ordersRepository.Delete(order);
            await this.ordersRepository.SaveChangesAsync();
            return order.Id;
        }

        public async Task AdminCompleteOrder(OrdersViewModel input)
        {
            Order dbOrder;
            foreach (var order in input.Orders)
            {
                dbOrder = this.ordersRepository.All()
                    .Where(x => x.Id == order.Id).FirstOrDefault();
                if (dbOrder == null || dbOrder.StatusId != (int)OrderStatus.Sent)
                {
                    throw new ArgumentException(nameof(dbOrder));
                }

                dbOrder.StatusId = (int)OrderStatus.Completed;
                dbOrder.DeliveryAddress = input.DeliveryAddress == null ? dbOrder.Orderer.Address : input.DeliveryAddress;
                this.ordersRepository.Update(dbOrder);
                await this.ordersRepository.SaveChangesAsync();
            }
        }

        public async Task SendOrderReceiptMailCustomer(string userId, string receiptId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var orders = this.ordersRepository.All()
                .Where(x => x.ReceiptId == receiptId).ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<table>
        <thead>
            <tr>
                <th class='col-md-3 text-left'>Поръчка №</th>
                <th class='col-md-1 text-left'>Дата</th>
                <th class='col-md-2 text-left'>Продукт</th>
                <th class='col-md-2 text-left'>Снимка</th>
                <th class='col-md-1 text-left'>Описание</th>
                <th class='col-md-1 text-left'>Количество</th>
                <th class='col-md-1 text-left'>Единична цена</th>
                <th class='col-md-1 text-left'>Крайна цена</th>
            </tr>
        </thead>
        <tbody>");
            foreach (var order in orders)
            {
                sb.AppendLine(
                    $@"<tr>
                <td class='col-md-3 text-left'>{order.Id}</td>
                <td class='col-md-1 text-left'>{order.IssuedOn}</td>
                <td class='col-md-2 text-left'>{order.MaterialName}</td>
                <td class='img-thumbnail product-cart-image'> <img src='{order.Image}'></img></td>                
                <td class='col-md-2 text-left'>{order.Description}</td>
                <td class='col-md-1 text-left'>{order.Quantity}</td>
                <td class='col-md-1 text-left'>{order.AvgPrice}</td>
                <td class='col-md-1 text-left'>{order.TotalPrice}</td>
            </tr>");
            }

            sb.AppendLine(@"</tbody>
                </ table > ");
            await this.sender.SendEmailAsync(GlobalConstants.SystemEmail, GlobalConstants.SystemName, user.Email, $"Поръчка по разписка " + receiptId, $"Здравейте, {user.UserName}, \nПолучихме от Вас следната поръчка" + sb.ToString() + $" за общо {orders.Sum(x => x.AvgPrice * (decimal)x.Quantity).ToString("f2")} лв. Ще Ви уведомим, когато е готова.\nПоздрави, {GlobalConstants.SystemName}");
        }
    }
}
