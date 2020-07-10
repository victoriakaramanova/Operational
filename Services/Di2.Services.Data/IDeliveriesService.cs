using Di2.Web.ViewModels.Deliveries;
using Di2.Web.ViewModels.OrderSuppliers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public interface IDeliveriesService
    {
        Task<int> Create(OrderSupplierViewModel input);

        //T GetById<T>(int materialId);

        T GetByMaterialId<T>(int materialId);

        IEnumerable<T> GetAllProducts<T>(int categoryId);

        bool GetDeliveredQuantityPerProduct(int materialId, double quantity);
    }
}
