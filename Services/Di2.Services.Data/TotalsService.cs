using Di2.Data.Common.Repositories;
using Di2.Data.Models;
using Di2.Data.Models.Enums;
using Di2.Web.ViewModels.OrderSuppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public class TotalsService : ITotalsService
    {
        private readonly IDeletableEntityRepository<OrderSupplier> orderSupplierRepository;
        private readonly IDeliveriesService deliveriesService;
        private readonly IRepository<SupplyOrderStatus> supplyOrderStatuses;
        private readonly IMaterialsService materialsService;

        public TotalsService(
            IDeletableEntityRepository<OrderSupplier> orderSupplierRepository,
            IDeliveriesService deliveriesService,
            IRepository<SupplyOrderStatus> supplyOrderStatuses,
            IMaterialsService materialsService)
        {
            this.orderSupplierRepository = orderSupplierRepository;
            this.deliveriesService = deliveriesService;
            this.supplyOrderStatuses = supplyOrderStatuses;
            this.materialsService = materialsService;
        }

        public int GetStatus(int orderId)
        {
            var status = this.supplyOrderStatuses.All()
                .FirstOrDefault(x => x.OrderId == orderId);
            var s = Enum.Parse(typeof(OrderStatus), status.ToString()) as Enum;
            return Convert.ToInt32(s);
        }

        public async Task<int> ChangeOrderStatus(int orderId, int isCompleted)
        {
            var supplyOrderStatus = this.supplyOrderStatuses.All()
                .FirstOrDefault(x => x.OrderId == orderId);

            var order = this.orderSupplierRepository.All()
                .FirstOrDefault(x => x.Id == orderId);
            var materialId = order.MaterialId;
            var material = this.materialsService.GetById(materialId);

            if (isCompleted == 0)
            {
                supplyOrderStatus.OrderStatus = OrderStatus.Completed;
                /*await this.supplyOrderStatuses.SaveChangesAsync();
                order.Status = supplyOrderStatus.OrderStatus;
                this.orderSupplierRepository.Update(order);
                await this.orderSupplierRepository.SaveChangesAsync();*/
                var deliveryId = await this.InvokeDelivery(order);
            }
            else
            if (isCompleted == -1)
            {
                //order.Status = OrderStatus.Canceled;
                supplyOrderStatus.OrderStatus = OrderStatus.Canceled;
            }
            else
            {
                //order.Status = OrderStatus.Sent;
                supplyOrderStatus.OrderStatus = OrderStatus.Sent;
            }

            await this.supplyOrderStatuses.SaveChangesAsync();
            order.Status = supplyOrderStatus.OrderStatus;
            this.orderSupplierRepository.Update(order);
            await this.orderSupplierRepository.SaveChangesAsync();

            return (int)order.Status;
        }

        public async Task<int> InvokeDelivery(OrderSupplier order)
        {
            /*var supplyOrderStatus = new SupplyOrderStatus
            {
                OrderId = order.Id,
                OrderStatus = order.Status,
            };
            await this.supplyOrderStatuses.AddAsync(supplyOrderStatus);
            */
            var viewModel = new OrderSupplierViewModel
            {
                Id = order.Id,
                MaterialId = order.MaterialId,
                SupplierId = order.SupplierId,
                OrderDate = order.OrderDate,
                Quantity = order.Quantity,
                UnitPrice = order.UnitPrice,
                TotalPrice = order.TotalPrice,
            };
            var deliveryId = await this.deliveriesService.Create(viewModel);
            return deliveryId;
        }

    }
}
