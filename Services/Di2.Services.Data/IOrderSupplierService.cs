namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Di2.Data.Models;
    using Di2.Web.ViewModels.OrderSuppliers;
    using Di2.Web.ViewModels.PriceLists.ViewModels;

    public interface IOrderSupplierService
    {
        Task<OrderSupplier> CreateAsync(CreateOrderSupplierInputModel orderSupplierinput, PriceListViewModel priceListInput, string userId);

        // Task<int> CreateAsync(DateTime orderDate, int materialId, int supplierId, double quantity, decimal unitPrice, decimal totalPrice, string userId);
        IEnumerable<T> GetAllOrderSuppliers<T>();

        Task SendMailSupplier(List<OrderSupplier> orderSuppliers);

        IEnumerable<T> GetByCategoryId<T>(int categoryId);

        int GetCount();

        Task DeleteAsync(int materialId, int supplierId, double qty, decimal unitPrice, decimal totalprice, DateTime odate);
    }
}
