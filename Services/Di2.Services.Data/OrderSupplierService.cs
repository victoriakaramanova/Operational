namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Di2.Common;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Data.Models.Enums;
    using Di2.Services.Mapping;
    using Di2.Services.Messaging;
    using Di2.Web.ViewModels.OrderSuppliers;
    using Di2.Web.ViewModels.PriceLists.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class OrderSupplierService : IOrderSupplierService
    {
        private readonly IDeletableEntityRepository<OrderSupplier> orderSuppliersRepository;
        private readonly IDeletableEntityRepository<PriceList> priceListRepository;
        private readonly IEmailSender sender;
        private readonly IDeletableEntityRepository<Material> materialsRepository;
        private readonly IDeletableEntityRepository<SupplyOrderStatus> supplyOrderStatuses;

        public OrderSupplierService(
            IDeletableEntityRepository<OrderSupplier> orderSuppliersRepository,
            IDeletableEntityRepository<PriceList> priceListRepository,
            IEmailSender sender,
            IDeletableEntityRepository<Material> materialsRepository,
            IDeletableEntityRepository<SupplyOrderStatus> supplyOrderStatuses)
        {
            this.orderSuppliersRepository = orderSuppliersRepository;
            this.priceListRepository = priceListRepository;
            this.sender = sender;
            this.materialsRepository = materialsRepository;
            this.supplyOrderStatuses = supplyOrderStatuses;
        }

        public async Task<OrderSupplier> CreateAsync(CreateOrderSupplierInputModel orderSupplierInput, PriceListViewModel priceListInput, string userId)
        {
            if (orderSupplierInput.OrderDate == null)
            {
                return null;
            }

            double number;
            Material material = this.materialsRepository.All()
                .FirstOrDefault(x => x.Id == priceListInput.MaterialId);
            double.TryParse(orderSupplierInput.Quantity.ToString(), out number);
            var orderSupplier = new OrderSupplier
            {
                MaterialId = priceListInput.MaterialId,
                Material = material,
                SupplierId = priceListInput.SupplierId,
                OrderDate = orderSupplierInput.OrderDate,
                Quantity = number,
                UnitPrice = priceListInput.UnitPrice,
                TotalPrice = priceListInput.UnitPrice * (decimal)orderSupplierInput.Quantity,
                UserId = userId,
            };

            orderSupplier.Status = OrderStatus.Sent;

            await this.orderSuppliersRepository.AddAsync(orderSupplier);
            await this.orderSuppliersRepository.SaveChangesAsync();
            SupplyOrderStatus supplyOrderStatus = new SupplyOrderStatus
            {
                OrderId = orderSupplier.Id,
                OrderStatus = OrderStatus.Sent,
            };
            await this.supplyOrderStatuses.AddAsync(supplyOrderStatus);
            await this.supplyOrderStatuses.SaveChangesAsync();
            return orderSupplier;
        }

        public async Task SendMailSupplier(List<OrderSupplier> orderSuppliers)
        {
            foreach (var supplier in orderSuppliers.GroupBy(x => x.SupplierId))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Дата за изпълнение;Име на материал;Описание на материал;Количество;Единична цена;Крайна цена" + Environment.NewLine);

                foreach (var material in supplier)
                {
                    sb.AppendFormat(
                            "{0};{1};{2};{3};{4};{5};{6}",
                            material.OrderDate.ToShortDateString(),
                            material.Material.Name,
                            material.Material.Description,
                            material.Quantity,
                            material.UnitPrice,
                            material.TotalPrice,
                            Environment.NewLine);
                }

                var data = Encoding.UTF8.GetBytes(sb.ToString());
                var res = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
                var attachmentFileName = $"{GlobalConstants.SystemName} - {DateTime.UtcNow.ToShortDateString()}.csv";
                var mimeType = "text/csv"; // charset=UTF-8

                var attch = new EmailAttachment
                {
                    MimeType = mimeType,
                    FileName = attachmentFileName,
                    Content = res,
                };
                var attchList = new List<EmailAttachment>();
                attchList.Add(attch);

                await this.sender.SendEmailAsync(GlobalConstants.SystemEmail, GlobalConstants.SystemName, supplier.Select(x => x.Supplier.Email).FirstOrDefault(), $"Поръчка за {DateTime.UtcNow.ToShortDateString()}", $"Здравейте, приложена е поръчката. Поздрави - {GlobalConstants.SystemName}", attchList);
            }
        }

        public IEnumerable<T> GetAllOrderSuppliers<T>()
        {
            IQueryable<OrderSupplier> query = this.orderSuppliersRepository.All();
            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetByCategoryId<T>(int categoryId)
        {
            var query = this.orderSuppliersRepository.All()
                .Where(x => x.Material.CategoryId == categoryId);

            return query.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.orderSuppliersRepository.All()
                .Where(x=>x.Status==OrderStatus.Sent)
                .Count();
        }

        public async Task DeleteAsync(int materialId, int supplierId, double qty, decimal unitPrice,decimal totalprice,DateTime odate)
        {
            var qtyDb = this.orderSuppliersRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId).Quantity;
            var unPrice = this.orderSuppliersRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId).UnitPrice;
            var totPrice = this.orderSuppliersRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId).TotalPrice;
            var orderDate = this.orderSuppliersRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId).OrderDate;
            var osToDelete = this.orderSuppliersRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId);
            if (osToDelete.Quantity == qtyDb && osToDelete.UnitPrice == unPrice && osToDelete.TotalPrice==totPrice && osToDelete.OrderDate==orderDate)
            {
                this.orderSuppliersRepository.Delete(osToDelete);
            }

            await this.orderSuppliersRepository.SaveChangesAsync();
        }
    }
}
