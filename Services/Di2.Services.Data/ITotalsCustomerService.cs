using Di2.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public interface ITotalsCustomerService
    {
        int GetStatus(string orderId);

        Task<int> ChangeOrderStatus(string orderId, int isCompleted, ApplicationUser orderer);

        bool IsAvailableQtyEnough(Order order);

        Task<double> DecreaseDeliveriesAsync(Order order);
    }
}
