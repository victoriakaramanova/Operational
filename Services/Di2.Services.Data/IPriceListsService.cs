namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Di2.Data.Models;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.PriceLists.InputModels;

    public interface IPriceListsService
    {
        Task CreateAsync(CreatePriceListInputModel input, string userId);

        List<T> GetAllPriceLists<T>();

        //IEnumerable<Material> GetAllMaterials<T>();

        //IEnumerable<Supplier> GetAllSuppliers<T>();

        T GetById<T>();

        T GetByElements<T>(int materialId, int supplierId, double minQty, decimal unitPrice);

        int GetCount();

        Task DeleteAsync(int materialId, int supplierId, double mqo, decimal unitPrice);
    }
}
