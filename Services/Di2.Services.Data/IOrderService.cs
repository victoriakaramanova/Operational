using Di2.Data.Models;
using Di2.Web.ViewModels.Orders.InputModels;
using Di2.Web.ViewModels.Orders.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public interface IOrderService
    {
        Task<int> CreateOrder(OrderInputModel input, string userId, List<string> customerImages=null);

        List<T> GetAll<T>();

        Task UpdateOrder(OrdersViewModel input);

        Task CompleteOrder(OrdersViewModel input);

        IEnumerable<T> GetReceiptOrders<T>(string receiptId);

        IEnumerable<T> GetReceiptOrdersByReceiptId<T>(string receiptId, string userId);

        Task<string> CreateReceipt(string recipientId, string deliveryAddress);

        T GetById<T>(string id);

        string GetRecipientName(string receiptId);

        int GetCount();

        Task<string> DeleteAsync(string id);

        Task AdminCompleteOrder(OrdersViewModel input);

        Task<int> AssignReceiptToOrders(string receiptId);

        Task SendOrderReceiptMailCustomer(string userId, string receiptId);
    }
}
